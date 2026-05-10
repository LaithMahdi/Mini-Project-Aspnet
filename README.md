# School Management System

Système de gestion scolaire ASP.NET Core MVC premium permettant une gestion complète et automatisée des étudiants, enseignants, classes et sessions.

## 🚀 Fonctionnalités Avancées

- ✅ **Système de Notifications** : Alertes en temps réel pour les affectations, transferts et annulations de sessions.
- ✅ **Gestion "Pro" des Classes** : Interface d'enrôlement avec recherche en temps réel, contrôle de capacité et gestion des transferts.
- ✅ **Planning Intelligent** : Gestion des sessions avec créneaux horaires fixes et détection de conflits.
- ✅ **Design Premium** : Interface moderne avec Glassmorphism, animations fluides et palettes de couleurs harmonieuses.
- ✅ **Gestion Multi-Rôles** : Tableaux de bord et accès spécifiques pour Administrateurs, Enseignants et Étudiants.
- ✅ **Validation Avancée** : Contrôles de données stricts côté serveur et client (dates, formats, capacités).

## 📋 Prérequis

- .NET 10 SDK ou supérieur
- Visual Studio 2022 ou VS Code
- Git
- SQL Server (LocalDB ou Express)
- Node.js (pour la compilation Tailwind)

## 🔧 Installation

### 1. Cloner le dépôt

```bash
git clone https://github.com/LaithMahdi/mini-project-aspnet.git
cd mini-project-aspnet
```

### 2. Configurer la base de données

L'application utilise une base de données SQL Server locale. Les migrations sont déjà incluses.

```bash
dotnet ef database update
```

### 3. Lancer l'application

```bash
dotnet run
```

L'application sera accessible sur `https://localhost:5001`. Les données de test sont automatiquement générées via l' `AppSeeder`.

## 📁 Structure du projet

```
school/
├── Controllers/
│   ├── NotificationsController.cs  # Gestion des alertes
│   ├── ClassesController.cs        # Gestion Pro des classes
│   ├── SessionsController.cs       # Gestion du planning
│   └── ...                         # Autres contrôleurs (Users, Students...)
├── Services/
│   ├── NotificationService.cs      # Logique d'envoi d'alertes
│   └── SessionScheduleService.cs   # Logique des créneaux horaires
├── Models/
│   ├── Notification.cs             # Modèle d'alerte
│   ├── Classe.cs                   # Modèle classe (avec capacité)
│   └── ...                         # Entités de base
├── ViewComponents/
│   └── NotificationCount.cs        # Badge dynamique dans la navbar
├── Views/
│   ├── Notifications/              # Interface des alertes
│   ├── Sessions/                   # Calendrier et planning
│   └── ...                         # Vues modules
├── wwwroot/
│   ├── css/modules/                # Styles spécifiques par module
│   └── js/                         # Scripts interactifs
└── README.md                       # Ce fichier
```

## 📝 Nouvelles Fonctionnalités "Pro"

### 🔔 Système de Notification

- **Alertes Automatiques** : Les enseignants sont notifiés lors de l'affectation d'un nouvel étudiant.
- **Gestion des Sessions** : Les administrateurs reçoivent une alerte immédiate si une session est annulée par un enseignant.
- **Interface Interactive** : Badge de comptage dynamique et page de gestion avec filtres (Tout, Non lu, Lu).

### 🎓 Enrôlement Avancé

- **Recherche en temps réel** : Filtrage instantané des étudiants lors de l'affectation.
- **Gestion des transferts** : Visualisation de la classe actuelle des étudiants et transfert automatique.
- **Contrôle de Capacité** : Indicateur visuel `[Sélection] / [Capacité]` avec blocage automatique si la limite est dépassée.

## 🛠️ Technologies utilisées

| Technologie           | Version | Utilisation                    |
| --------------------- | ------- | ------------------------------ |
| ASP.NET Core          | 10      | Framework web                  |
| C#                    | 14.0    | Langage principal              |
| Entity Framework Core | 10.0.6  | ORM base de données            |
| Razor View Components | -       | UI dynamique et réutilisable   |
| Tailwind CSS          | -       | Design premium                 |
| jQuery / AJAX         | -       | Interactions sans rechargement |

## 📋 Checklist de développement

- [x] Système de Notifications temps réel
- [x] Enrôlement Pro avec contrôle de capacité
- [x] Planning par créneaux horaires
- [x] Validation avancée (regex, dates passées/futures)
- [x] Redesign complet de l'interface (Login, Sessions, Notifs)
- [x] Seeding de données complexe
- [ ] Exports PDF des emplois du temps
- [ ] Messagerie interne entre enseignants

## 👨‍💻 Auteur

**Laith Mahdi**

- GitHub : [@LaithMahdi](https://github.com/LaithMahdi)

---

**Dernière mise à jour** : 2026  
**Version** : 1.2.0  
**Statut** : Stable - En expansion 🚀

```bash
# Restaurer les dépendances
dotnet restore

# Installer les dépendances front-end (Tailwind)
npm install

# Build le projet
dotnet build

# Lancer en debug
dotnet run

# Compiler Tailwind CSS
npm run tailwind:build

# Migrations EF Core
dotnet ef migrations add NomMigration
dotnet ef database update
dotnet ef database drop

# Migrations EF Core (NuGet Package Manager Console)
PM> Drop-Database
PM> Add-Migration UpdateFields
PM> Update-Database

# Tests
dotnet test
```
