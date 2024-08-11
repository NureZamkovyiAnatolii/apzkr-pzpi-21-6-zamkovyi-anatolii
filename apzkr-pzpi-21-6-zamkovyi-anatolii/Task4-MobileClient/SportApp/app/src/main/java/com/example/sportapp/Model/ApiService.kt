package com.example.sportapp.Model

import android.util.Log
import com.example.sportapp.ui.home.LoginRequest
import com.example.sportapp.ui.home.LoginResponse
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Headers
import retrofit2.http.POST
import retrofit2.http.Query
import java.security.cert.CertificateException
import java.util.Date
import javax.net.ssl.SSLContext
import javax.net.ssl.TrustManager
import javax.net.ssl.X509TrustManager
import com.google.gson.annotations.SerializedName
import com.google.gson.reflect.TypeToken
import com.google.gson.Gson
import com.google.gson.*
import retrofit2.http.Path
import java.lang.reflect.Type
import java.text.SimpleDateFormat
import java.util.*
interface ApiService {
    @Headers("Content-Type: application/json", "accept: text/plain")
    @POST("/api/Login")
    suspend fun login(@Body request: LoginRequest): Response<LoginResponse>

    @Headers("Content-Type: application/json", "accept: text/plain")
    @POST("/api/Coaches")
    suspend fun registerCoach(@Body request: CoachRegistrationRequest): Response<CoachRegistrationResponse>

    @Headers("Content-Type: application/json", "accept: text/plain")
    @POST("/api/Athletes")
    suspend fun registerAthlete(@Body request: AthleteRegistrationRequest): Response<AthleteRegistrationResponse>

    @GET("/api/Athletes/Me")
    suspend fun getMyAthleteInfo(): Response<Athlete>

    @GET("/api/Coaches/Me")
    suspend fun getMyCoachInfo(): Response<Coach>

    @Headers("Accept: application/json")
    @GET("/api/Coaches/GetAthletesByCoach")
    suspend fun getMyAthletes(@Query("coachId") coachId: Int): Response<List<Athlete>>

    @Headers("Accept: application/json")
    @GET("/api/Athletes/GetCompetitionsByAthletes")
    suspend fun getAthletesCompetitions(@Query("athleteId") athleteId: Int): Response<CompetitionsResponse>

    @Headers("Accept: application/json")
    @GET("/api/Athletes/{id}")
    suspend fun getAthleteById(@Path("id") athleteId: Int): Response<Athlete>

    @GET("/api/Coaches")
    suspend fun getCoaches(): Response<List<Coach>>

    @POST("/api/Login/logout")
    suspend fun logout(): Response<Void>
}

fun getUnsafeOkHttpClient(): OkHttpClient {
    return try {
        val trustAllCerts = arrayOf<TrustManager>(
            object : X509TrustManager {
                @Throws(CertificateException::class)
                override fun checkClientTrusted(chain: Array<java.security.cert.X509Certificate>, authType: String) {}

                @Throws(CertificateException::class)
                override fun checkServerTrusted(chain: Array<java.security.cert.X509Certificate>, authType: String) {}

                override fun getAcceptedIssuers(): Array<java.security.cert.X509Certificate> {
                    return arrayOf()
                }
            }
        )

        val sslContext = SSLContext.getInstance("SSL")
        sslContext.init(null, trustAllCerts, java.security.SecureRandom())
        val sslSocketFactory = sslContext.socketFactory

        val logging = HttpLoggingInterceptor().apply {
            level = HttpLoggingInterceptor.Level.BODY
        }

        val builder = OkHttpClient.Builder()
            .sslSocketFactory(sslSocketFactory, trustAllCerts[0] as X509TrustManager)
            .hostnameVerifier { _, _ -> true }
            .addInterceptor(logging)

        builder.build()
    } catch (e: Exception) {
        throw RuntimeException(e)
    }
}




