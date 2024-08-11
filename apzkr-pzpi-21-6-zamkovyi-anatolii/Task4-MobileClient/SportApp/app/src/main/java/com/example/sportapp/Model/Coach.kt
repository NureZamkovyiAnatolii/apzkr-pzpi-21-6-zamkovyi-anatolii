package com.example.sportapp.Model

import com.google.gson.annotations.SerializedName

data class Coach(
    @SerializedName("coachId") val coachId: Int,
    @SerializedName("coachName") val coachName: String,
    @SerializedName("password") val password: String,
    @SerializedName("birthDate") val birthDate: String,
    @SerializedName("phoneNumber") val phoneNumber: Int,
    @SerializedName("country") val country: String
)
data class CoachRegistrationRequest(
    @SerializedName("coachName") val coachName: String,
    @SerializedName("password") val password: String,
    @SerializedName("birthDate") val birthDate: String,
    @SerializedName("phoneNumber") val phoneNumber: Int,
    @SerializedName("country") val country: String
)
data class CoachRegistrationResponse(
    @SerializedName("message") val message: String
)