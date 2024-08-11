package com.example.sportapp.ui.home

import android.app.DatePickerDialog
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.lifecycleScope
import com.example.sportapp.databinding.FragmentHomeBinding
import com.google.gson.Gson
import com.google.gson.annotations.SerializedName
import kotlinx.coroutines.launch
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.Headers
import retrofit2.http.POST
import retrofit2.http.GET
import java.security.cert.CertificateException
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Date
import java.util.Locale
import javax.net.ssl.*
import com.example.sportapp.Model.*

data class LoginRequest(
    val role: String,
    val name: String,
    val password: String
)
public

data class LoginResponse(
    @SerializedName("message") val message: String,
    @SerializedName("userId") val userId: Int
)

class HomeFragment : Fragment() {

    private var _binding: FragmentHomeBinding? = null
    private val binding get() = _binding!!

    private var isRegistering = false
    private var isRegisteringCoach = true // Flag to determine if registering a coach

    private val api: ApiService= AppSettings.api

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View {
        val homeViewModel = ViewModelProvider(this).get(HomeViewModel::class.java)

        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        val root: View = binding.root

        homeViewModel.text.observe(viewLifecycleOwner) {
            binding.textHome.text = it
        }

        setupUI()
        setupListeners()

        return root
    }
    private fun setupListeners() {
        binding.buttonSubmit.setOnClickListener { handleSubmitButton() }
        binding.textSwitch.setOnClickListener { handleSwitchText() }
        binding.spinnerRole.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                isRegisteringCoach = binding.spinnerRole.selectedItem == "Тренер"
                setupUI()
            }
            override fun onNothingSelected(parent: AdapterView<*>?) {}
        }
        binding.editTextBirthDate.setOnClickListener { showDatePicker() }
        binding.buttonLogout.setOnClickListener { handleLogout() }
    }

    private fun handleSubmitButton() {
        val username = binding.editTextUsername.text.toString()
        val password = binding.editTextPassword.text.toString()
        val confirmPassword = binding.editTextConfirmPassword.text.toString()
        val birthDate = formatToISO8601(binding.editTextBirthDate.text.toString())
        val phoneNumber = binding.editTextPhoneNumber.text.toString()
        val country = binding.editTextCountry.text.toString()
        val role = binding.spinnerRole.selectedItem.toString()
        val coachId = getSelectedCoachId()

        if (isRegistering) {
            if (password != confirmPassword) {
                showText("Passwords do not match")
                return
            }
            val request = if (isRegisteringCoach) {
                CoachRegistrationRequest(username, password, birthDate, phoneNumber.toInt(), country)
            } else {
                AthleteRegistrationRequest(username, password, birthDate, phoneNumber.toInt(), coachId)
            }
            lifecycleScope.launch { handleRegistration(request) }
        } else {
            val request = LoginRequest(role, username, password)
            lifecycleScope.launch { handleLogin(request) }
        }
    }

    private fun handleSwitchText() {
        isRegistering = !isRegistering
        setupUI()

    }
    private fun getSelectedCoachId(): Int? {
        return if (isRegistering && binding.spinnerCoaches.visibility == View.VISIBLE) {
            val selectedCoachName = binding.spinnerCoaches.selectedItem as? String
            val selectedCoach = coaches.find { it.coachName == selectedCoachName }
            selectedCoach?.coachId
        } else null
    }

    private suspend fun handleLogin(request: LoginRequest) {
        try {
            val response = api.login(request)
            if (response.isSuccessful) {
                val message = response.body()?.message ?: "Unexpected response"
                handleSuccessfulLogin(request.name, request.role)
                UserSession.username = request.name
                UserSession.role = request.role
                UserSession.id = response.body()?.userId
                Log.d("Login ",message+UserSession.id )
            } else {
                val errorBody = response.errorBody()?.string()
                val errorMessage = parseErrorMessage(errorBody)
                showText("Error: $errorMessage")
            }
        } catch (e: Exception) {
            showText("Exception: ${e.message}")
        }
    }
    private suspend fun handleRegistration(request: Any) {
        try {
            val response = if (isRegisteringCoach) {
                api.registerCoach(request as CoachRegistrationRequest)
            } else {
                api.registerAthlete(request as AthleteRegistrationRequest)
            }
            handleSuccessfulLogin((request as CoachRegistrationRequest).coachName, binding.spinnerRole.selectedItem.toString())
            if(isRegisteringCoach){
                var coach = api.getMyCoachInfo().body()
                if (coach != null) {
                    UserSession.id = coach.coachId
                }
            }
            val message = response.body()
            showText( "Registration successful"+UserSession.id)
        } catch (e: Exception) {
            showText("Error: ${e.message}")
        }
    }


    private fun showDatePicker() {
        val calendar = Calendar.getInstance()
        val year = calendar.get(Calendar.YEAR)
        val month = calendar.get(Calendar.MONTH)
        val day = calendar.get(Calendar.DAY_OF_MONTH)

        val datePickerDialog = DatePickerDialog(
            requireContext(),
            { _, selectedYear, selectedMonth, selectedDay ->
                val selectedDate = "${selectedDay}/${selectedMonth + 1}/${selectedYear}"
                binding.editTextBirthDate.setText(selectedDate)
            },
            year,
            month,
            day
        )
        datePickerDialog.show()
    }

    private fun setupUI() {
        binding.textSwitch.text = if (isRegistering) "Маєте аакаунт?Увійти" else "Не зареєстрвані? Створити аккаунт"
        binding.buttonSubmit.text = if (isRegistering) "Зареєструватися" else "Увійти"

        binding.editTextCountry.visibility = if (isRegistering) View.VISIBLE else View.GONE
        binding.editTextPhoneNumber.visibility = if (isRegistering) View.VISIBLE else View.GONE
        binding.editTextBirthDate.visibility = if (isRegistering) View.VISIBLE else View.GONE
        binding.editTextConfirmPassword.visibility = if (isRegistering) View.VISIBLE else View.GONE
        binding.spinnerCoaches.visibility = if (isRegistering && !isRegisteringCoach) View.VISIBLE else View.GONE
        // Debug log for tracking isRegisteringCoach state
        Log.d("isRegisteringCoach", isRegisteringCoach.toString())
        if (isRegistering && !isRegisteringCoach) {
            loadCoaches()
        } else {
            binding.spinnerCoaches.visibility = View.GONE
        }
        // Set the click listener for the logout button
        binding.buttonLogout.setOnClickListener {
            handleLogout()
        }
    }

    private fun handleLogout() {
        lifecycleScope.launch {
            try {
                val response = api.logout()
                if (response.isSuccessful) {
                    // If the server response is successful, update the UI
                    binding.textSuccessMessage.visibility = View.GONE
                    binding.buttonLogout.visibility = View.GONE
                    binding.editTextUsername.visibility = View.VISIBLE
                    binding.editTextPassword.visibility = View.VISIBLE
                    binding.editTextConfirmPassword.visibility = if (isRegistering) View.VISIBLE else View.GONE
                    binding.editTextCountry.visibility = if (isRegistering) View.VISIBLE else View.GONE
                    binding.editTextPhoneNumber.visibility = if (isRegistering) View.VISIBLE else View.GONE
                    binding.editTextBirthDate.visibility = if (isRegistering) View.VISIBLE else View.GONE
                    binding.spinnerRole.visibility = if (isRegistering) View.VISIBLE else View.GONE
                    binding.spinnerCoaches.visibility = if (isRegistering && !isRegisteringCoach) View.VISIBLE else View.GONE
                    binding.buttonSubmit.visibility = View.VISIBLE
                    binding.textSwitch.visibility = View.VISIBLE
                    showText("Вихід вдалий!")
                } else {
                    // Show error message
                    showText("Logout failed: ${response.message()}")
                }
            } catch (e: Exception) {
                // Show error message
                showText("Logout failed: ${e.message}")
            }
        }
    }
    private fun showText(message: String) {
        Toast.makeText(context, message, Toast.LENGTH_LONG).show()
    }

    private fun handleSuccessfulLogin(username: String, role: String) {
        UserSession.username = username
        UserSession.role = role

        binding.textSuccessMessage.visibility = View.VISIBLE
        binding.textSuccessMessage.text = "ви зареєстровані як $username ($role)"
        binding.editTextUsername.visibility = View.GONE
        binding.editTextPassword.visibility = View.GONE
        binding.editTextConfirmPassword.visibility = View.GONE
        binding.editTextCountry.visibility = View.GONE
        binding.editTextPhoneNumber.visibility = View.GONE
        binding.editTextBirthDate.visibility = View.GONE
        binding.spinnerRole.visibility = View.GONE
        binding.spinnerCoaches.visibility = View.GONE
        binding.buttonSubmit.visibility = View.GONE
        binding.textSwitch.visibility = View.GONE
        binding.buttonLogout.visibility = View.VISIBLE
    }

    private var coaches: List<Coach> = emptyList() // Додайте змінну для зберігання тренерів

    private fun loadCoaches() {
        lifecycleScope.launch {
            try {
                val response = api.getCoaches()
                if (response.isSuccessful) {
                    coaches = response.body() ?: emptyList() // Зберігаємо список тренерів
                    val coachNames = coaches.map { it.coachName } // Створіть список імен тренерів

                    val adapter = ArrayAdapter(
                        requireContext(),
                        android.R.layout.simple_spinner_item,
                        coachNames // Використовуйте список імен тренерів
                    )
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
                    binding.spinnerCoaches.adapter = adapter

                    // Налаштуйте слухача вибору
                    setupSpinnerListener()

                } else {
                    Toast.makeText(requireContext(), "Не вдалося завантажити тренерів", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Помилка: ${e.message}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun setupSpinnerListener() {
        binding.spinnerCoaches.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>, view: View?, position: Int, id: Long) {
                val selectedCoachName = parent.getItemAtPosition(position) as String
                val selectedCoach = coaches.find { it.coachName == selectedCoachName }
                // Тепер ви можете використовувати selectedCoach для подальших дій
            }

            override fun onNothingSelected(parent: AdapterView<*>) {
                // Дії при відсутності вибору
            }
        }
    }




    private fun parseErrorMessage(errorBody: String?): String {
        return try {
            val gson = Gson()
            val errorObject = gson.fromJson(errorBody, Map::class.java)
            errorObject["message"] as? String ?: "Unknown error"
        } catch (e: Exception) {
            "Parsing error message failed"
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
