package com.example.sportapp.ui.profile

import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.sportapp.Model.*
import com.example.sportapp.R
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import retrofit2.HttpException

class AthleteAdapter(
    private var athletes: List<Athlete>,
    private val api: ApiService
) : RecyclerView.Adapter<AthleteAdapter.AthleteViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): AthleteViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.item_athlete, parent, false)
        return AthleteViewHolder(view)
    }

    override fun onBindViewHolder(holder: AthleteViewHolder, position: Int) {
        val athlete = athletes[position]
        holder.nameTextView.text = athlete.athleteName
        holder.birthDateTextView.text = formatToISO8601(athlete.birthDate)

        // Manage the details visibility state
        var isDetailsVisible = false

        holder.moreDetailsButton.setOnClickListener {
            if (isDetailsVisible) {
                // Hide detailed information
                holder.competitionsTextView.visibility = View.GONE
                holder.moreDetailsButton.text = "Детальніше"
                isDetailsVisible = false
            } else {
                // Show detailed information
                CoroutineScope(Dispatchers.IO).launch {
                    try {
                        Log.d("AthleteAdapter", "Fetching competitions for athleteId: ${athlete.athleteId}")
                        val response = athlete.athleteId?.let { id -> api.getAthletesCompetitions(id) }
                        if (response != null) {
                            if (response.isSuccessful) {
                                val competitionsResponse = response.body()
                                val futureCompetitions = competitionsResponse?.futureCompetitions ?: emptyList()
                                val pastCompetitions = competitionsResponse?.results ?: emptyList()

                                Log.d("AthleteAdapter", "Competitions fetched successfully: $futureCompetitions, $pastCompetitions")

                                withContext(Dispatchers.Main) {
                                    val futureCompetitionsText = futureCompetitions.joinToString("\n\n") { competition ->
                                        val crewsInfo = competition.crews.joinToString("\n") { crew ->
                                            val athletesInfo = crew.athletes.joinToString(", ") { athlete -> athlete.athleteName }
                                            "       Crew Start Number: ${crew.startNumber}\n" +
                                                    "       Boat Type: ${crew.boatType}\n" +
                                                    "       Athletes: $athletesInfo\n"
                                        }
                                        "   Competition Name: ${competition.competitionName}\n" +
                                                "   City: ${competition.competitionCity}\n" +
                                                "   Country: ${competition.competitionCountry}\n" +
                                                "   Crews:\n$crewsInfo"
                                    }

                                    val pastCompetitionsText = pastCompetitions.joinToString("\n\n") { competition ->
                                        val crewsInfo = competition.crews.joinToString("\n") { crew ->
                                            val athletesInfo = crew.athletes.joinToString(", ") { athlete -> athlete.athleteName }
                                            "    Crew Start Number: ${crew.startNumber}\n" +
                                                    "   Boat Type: ${crew.boatType}\n" +
                                                    "   Athletes: $athletesInfo\n"+
                                                    "       Time Taken: ${crew.timeTaken}"
                                        }
                                        "   Competition Name: ${competition.competitionName}\n" +
                                                "   City: ${competition.competitionCity}\n" +
                                                "   Country: ${competition.competitionCountry}\n" +
                                                "   Crews:\n$crewsInfo"
                                    }

                                    holder.competitionsTextView.text = "Future Competitions:\n$futureCompetitionsText\n\nPast Competitions:\n$pastCompetitionsText"
                                    holder.competitionsTextView.visibility = View.VISIBLE
                                    holder.moreDetailsButton.text = "Менше"
                                    isDetailsVisible = true
                                }
                            } else {
                                Log.e("AthleteAdapter", "Failed to fetch competitions. Response code: ${response.code()}")
                                withContext(Dispatchers.Main) {
                                    holder.competitionsTextView.text = "Не вдалося завантажити змагання."
                                    holder.competitionsTextView.visibility = View.VISIBLE
                                    holder.moreDetailsButton.text = "Менше"
                                    isDetailsVisible = true
                                }
                            }
                        } else {
                            Log.e("AthleteAdapter", "Response is null for athleteId: ${athlete.athleteId}")
                            withContext(Dispatchers.Main) {
                                holder.competitionsTextView.text = "Не вдалося завантажити змагання."
                                holder.competitionsTextView.visibility = View.VISIBLE
                                holder.moreDetailsButton.text = "Менше"
                                isDetailsVisible = true
                            }
                        }
                    } catch (e: HttpException) {
                        Log.e("AthleteAdapter", "HttpException: ${e.message()}", e)
                        withContext(Dispatchers.Main) {
                            holder.competitionsTextView.text = "Помилка мережі: ${e.message}"
                            holder.competitionsTextView.visibility = View.VISIBLE
                            holder.moreDetailsButton.text = "Менше"
                            isDetailsVisible = true
                        }
                    } catch (e: Exception) {
                        Log.e("AthleteAdapter", "Exception: ${e.message}", e)
                        withContext(Dispatchers.Main) {
                            holder.competitionsTextView.text = "Помилка: ${e.message}"
                            holder.competitionsTextView.visibility = View.VISIBLE
                            holder.moreDetailsButton.text = "Менше"
                            isDetailsVisible = true
                        }
                    }
                }
            }
        }
    }

    override fun getItemCount(): Int = athletes.size

    fun updateAthletes(newAthletes: List<Athlete>) {
        athletes = newAthletes
        notifyDataSetChanged()
    }

    class AthleteViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val nameTextView: TextView = itemView.findViewById(R.id.athleteNameTextView)
        val birthDateTextView: TextView = itemView.findViewById(R.id.birthDateTextView)
        val moreDetailsButton: Button = itemView.findViewById(R.id.moreDetailsButton)
        val competitionsTextView: TextView = itemView.findViewById(R.id.competitionsTextView)
    }
}

