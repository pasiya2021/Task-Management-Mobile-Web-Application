# TaskFlowManager.Mobile

A cross-platform .NET MAUI app that wraps your unique Vue.js Task Management frontend in a native WebView, with a custom splash screen and color palette.

---

## Features
- Loads your deployed Vue.js frontend inside a native WebView
- Custom splash screen (teal/amber palette, bold "T" logo)
- Simple, fast, and cross-platform (Android, iOS, Windows, Mac Catalyst)
- Easy to update for new frontend deployments

---

## Setup & Usage

### Prerequisites
- .NET 7+ SDK with MAUI workload installed ([MAUI Install Guide](https://learn.microsoft.com/en-us/dotnet/maui/installation))
- Android/iOS/Windows/Mac build tools

### Steps
1. Open the `TaskFlowManager.Mobile` folder in Visual Studio 2022 or VS Code.
2. Restore dependencies:
   ```
   dotnet restore
   ```
3. Build the app:
   ```
   dotnet build
   ```
4. Run on your target platform:
   ```
   dotnet run -f net7.0-android
   # or net7.0-ios, net7.0-windows10.0.19041.0, etc.
   ```

---

## WebView URL
- The WebView loads your deployed Vue.js frontend:
  ```xml
  <WebView x:Name="TaskWebView" Source="https://your-taskmanager-frontend.netlify.app/" ... />
  ```
- Change the `Source` in `MainPage.xaml` if your deployed URL is different.

---

## Customization
- **Splash Screen:** Replace `Resources/Splash/splash.svg` with your own logo/design.
- **Colors:** Edit `App.xaml` for palette tweaks.
- **Native Features:** Add more native pages or features as needed (offline, notifications, etc).

---

## Troubleshooting
- **WebView not loading:** Ensure your frontend is deployed and accessible from your device/emulator.
- **Network errors:** Update the WebView URL if your frontend address changes.
- **Build errors:** Make sure all MAUI workloads and required SDKs are installed.

---

## Project Structure
```
TaskFlowManager.Mobile/
├── App.xaml / App.xaml.cs
├── MainPage.xaml / MainPage.xaml.cs
├── MauiProgram.cs
├── TaskFlowManager.Mobile.csproj
├── Resources/
│   └── Splash/splash.svg
└── Platforms/
```

---

## Credits
- Built by [Your Name] for a unique, modern task management experience.
