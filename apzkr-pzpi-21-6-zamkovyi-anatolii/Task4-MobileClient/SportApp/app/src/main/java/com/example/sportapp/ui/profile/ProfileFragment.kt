package com.example.sportapp.ui.profile

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.sportapp.Model.*
import com.example.sportapp.databinding.FragmentProfileBinding
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext

class ProfileFragment : Fragment() {

    private var _binding: FragmentProfileBinding? = null
    private val binding get() = _binding!!

    private val api: ApiService= AppSettings.api

    private lateinit var recyclerView: RecyclerView
    private lateinit var athleteAdapter: AthleteAdapter

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val galleryViewModel =
            ViewModelProvider(this).get(GalleryViewModel::class.java)

        _binding = FragmentProfileBinding.inflate(inflater, container, false)
        val root: View = binding.root

        // Set up RecyclerView and Adapter
        recyclerView = binding.athleteRecyclerView
        recyclerView.layoutManager = LinearLayoutManager(context)

        // Create the adapter with the API service instance
        athleteAdapter = AthleteAdapter(emptyList(), api)
        recyclerView.adapter = athleteAdapter

        val textView: TextView = binding.textGallery
        galleryViewModel.text.observe(viewLifecycleOwner) {
            textView.text = it
        }

        // Check the role and fetch athletes accordingly
        if (UserSession.role == "Тренер") {
            fetchAllAthletes()
        } else if (UserSession.role == "Спортсмен") {
            fetchSingleAthlete()
        } else {
            textView.text = "You are not authorized to view this information."
        }

        return root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    private fun fetchAllAthletes() {
        CoroutineScope(Dispatchers.IO).launch {
            try {
                val response = UserSession.id?.let { api.getMyAthletes(it) }
                if (response != null) {
                    if (response.isSuccessful) {
                        val athletes = response.body() ?: emptyList()
                        withContext(Dispatchers.Main) {
                            athleteAdapter.updateAthletes(athletes)
                        }
                    } else {
                        withContext(Dispatchers.Main) {
                            Toast.makeText(context, "Failed to fetch athletes", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    Toast.makeText(context, "An error occurred: ${e.message}", Toast.LENGTH_SHORT).show()
                    Log.e("GalleryFragment", "Exception: ${e.message}", e)
                }
            }
        }
    }

    private fun fetchSingleAthlete() {
        UserSession.id?.let { athleteId ->
            CoroutineScope(Dispatchers.IO).launch {
                try {
                    val response = api.getAthleteById(athleteId)
                    if (response.isSuccessful) {
                        val athlete = response.body()
                        withContext(Dispatchers.Main) {
                            athlete?.let {
                                athleteAdapter.updateAthletes(listOf(it))
                            }
                        }
                    } else {
                        withContext(Dispatchers.Main) {
                            Toast.makeText(context, "Failed to fetch athlete", Toast.LENGTH_SHORT).show()
                        }
                    }
                } catch (e: Exception) {
                    withContext(Dispatchers.Main) {
                        Toast.makeText(context, "An error occurred: ${e.message}", Toast.LENGTH_SHORT).show()
                        Log.e("GalleryFragment", "Exception: ${e.message}", e)
                    }
                }
            }
        }
    }
}
