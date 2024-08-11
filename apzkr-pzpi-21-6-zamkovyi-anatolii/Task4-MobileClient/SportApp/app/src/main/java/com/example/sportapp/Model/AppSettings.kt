package com.example.sportapp.Model

import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object AppSettings {
    const val BASE_URL = "https://10.0.2.2:7237"
    val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL) // Update to your base URL
            .client(getUnsafeOkHttpClient()) // Ensure this is correctly implemented
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }
    val api: ApiService by lazy {
        retrofit.create(ApiService::class.java)
    }
}