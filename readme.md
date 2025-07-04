# Internship Project

This repository contains the code and documentation for my internship project.

## Overview

Basic geo-organize app.

## Data Access Layer

┌─────────────────────────────────────┐
│           Controller Layer          │  ← HTTP requests
├─────────────────────────────────────┤
│           Service Layer            │  ← Business logic
├─────────────────────────────────────┤
│          Repository Layer          │  ← Data operations
├─────────────────────────────────────┤
│       DbContext (DAL Layer)        │  ← ORM mapping
├─────────────────────────────────────┤
│         PostgreSQL Database        │  ← Data storage
└─────────────────────────────────────┘

## DbContext Workflow

1. HTTP Request → Controller
2. Controller → Service
3. Service → Repository  
4. Repository → DbContext.Points.Add()    // ← Data Access Layer
5. DbContext → SQL INSERT oluştur
6. PostgreSQL → Query execute et
7. PostgreSQL → Result döndür
8. DbContext → C# object'e map et
9. Repository → Service'e döndür
10. Response → Client
