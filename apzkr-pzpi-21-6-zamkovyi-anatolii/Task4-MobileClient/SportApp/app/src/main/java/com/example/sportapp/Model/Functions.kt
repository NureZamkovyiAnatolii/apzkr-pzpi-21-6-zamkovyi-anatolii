package com.example.sportapp.Model

import android.util.Log
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import com.google.gson.JsonDeserializationContext
import com.google.gson.JsonDeserializer
import com.google.gson.JsonElement
import com.google.gson.reflect.TypeToken
import java.lang.reflect.Type
import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale

class CompetitionsResponseDeserializer : JsonDeserializer<CompetitionsResponse> {
    private val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", Locale.getDefault())

    override fun deserialize(json: JsonElement, typeOfT: Type, context: JsonDeserializationContext): CompetitionsResponse {
        val jsonObject = json.asJsonObject

        val futureCompetitions = context.deserialize<List<CompetitionWithCrewsDto>>(jsonObject.get("futureCompetitions"), object : TypeToken<List<CompetitionWithCrewsDto>>() {}.type)
        val results = context.deserialize<List<CompetitionWithCrewsDto>>(jsonObject.get("results"), object : TypeToken<List<CompetitionWithCrewsDto>>() {}.type)

        return CompetitionsResponse(futureCompetitions, results)
    }
}
fun createGson(): Gson {
    return GsonBuilder()
        .registerTypeAdapter(CompetitionsResponse::class.java, CompetitionsResponseDeserializer())
        .create()
}
public fun formatToISO8601(date: String): String {
    val inputFormat = SimpleDateFormat("dd/MM/yyyy", Locale.getDefault())
    val outputFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", Locale.getDefault())
    return try {
        val parsedDate = inputFormat.parse(date)
        outputFormat.format(parsedDate ?: Date())
    } catch (e: Exception) {
        Log.e("DateFormat", "Error parsing date", e)
        ""
    }
}