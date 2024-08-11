package com.example.sportapp.Model

import com.google.gson.annotations.SerializedName




data class TimeTakenDto(
    @SerializedName("ticks") val ticks: Long,
    @SerializedName("days") val days: Int,
    @SerializedName("hours") val hours: Int,
    @SerializedName("milliseconds") val milliseconds: Int,
    @SerializedName("minutes") val minutes: Int,
    @SerializedName("seconds") val seconds: Int,
    @SerializedName("totalDays") val totalDays: Long,
    @SerializedName("totalHours") val totalHours: Long,
    @SerializedName("totalMilliseconds") val totalMilliseconds: Long,
    @SerializedName("totalMinutes") val totalMinutes: Long,
    @SerializedName("totalSeconds") val totalSeconds: Long
)

data class AthleteDto(
    @SerializedName("athleteId") val athleteId: Int,
    @SerializedName("athleteName") val athleteName: String
)

data class CrewDto(
    @SerializedName("crewId") val crewId: Int,
    @SerializedName("boatType") val boatType: BoatType,
    @SerializedName("competitionId") val competitionId: Int,
    @SerializedName("raceId") val raceId: Int,
    @SerializedName("startNumber") val startNumber: Int,
    @SerializedName("athletes") val athletes: List<AthleteDto>,
    @SerializedName("timeTaken") val timeTaken: String // Змінили тип на String
)

data class CompetitionWithCrewsDto(
    @SerializedName("competitionId") val competitionId: Int?,
    @SerializedName("organizationId") val organizationId: Int?,
    @SerializedName("coachId") val coachId: Int?,
    @SerializedName("competitionName") val competitionName: String,
    @SerializedName("competitionCountry") val competitionCountry: String,
    @SerializedName("competitionCity") val competitionCity: String,
    @SerializedName("crews") val crews: List<CrewDto>
)

data class CompetitionsResponse(
    @SerializedName("futureCompetitions") val futureCompetitions: List<CompetitionWithCrewsDto>,
    @SerializedName("results") val results: List<CompetitionWithCrewsDto>
)