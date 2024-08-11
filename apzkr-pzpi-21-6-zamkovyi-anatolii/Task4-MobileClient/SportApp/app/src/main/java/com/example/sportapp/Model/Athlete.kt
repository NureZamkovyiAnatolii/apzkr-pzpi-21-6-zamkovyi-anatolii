package com.example.sportapp.Model

import com.google.gson.annotations.SerializedName

data class Athlete(
    @SerializedName("athleteId") val athleteId: Int?,
    @SerializedName("athleteName") val athleteName: String,
    @SerializedName("password") val password: String,
    @SerializedName("birthDate") val birthDate: String,
    @SerializedName("phoneNumber") val phoneNumber: String,
    @SerializedName("coachId") val coachId: Int? // CoachId є необов'язковим полем
)
data class AthleteRegistrationRequest(
    @SerializedName("athleteName") val athleteName: String,
    @SerializedName("password") val password: String,
    @SerializedName("birthDate") val birthDate: String,
    @SerializedName("phoneNumber") val phoneNumber: Int,
    @SerializedName("coachId") val coachId: Int? // CoachId є необов'язковим полем
)

data class AthleteRegistrationResponse(
    @SerializedName("message") val message: String
)
