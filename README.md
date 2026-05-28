# MSP Technician
[![Ask DeepWiki](https://devin.ai/assets/askdeepwiki.png)](https://deepwiki.com/Red-Den-Software/MSP-Technician)

## Overview
MSP Technician is a comprehensive WPF desktop application built with .NET 8, designed to be an all-in-one toolkit for IT professionals. It aims to streamline and simplify common technical tasks, enhancing efficiency for Managed Service Provider (MSP) technicians. The application follows the MVVM design pattern for a clean and maintainable architecture.

## Features
-   **Live Disk Cloning**: Clone disks on a running system by leveraging the Windows Volume Shadow Copy Service (VSS), ensuring data integrity without service interruption.
-   **User Data Transfer**: Seamlessly transfer user profile data to an external drive. The application organizes the information for easy migration.
-   **Network Configuration**: Quickly change a Windows user's network connection type to 'Private' to enable network sharing and discovery.
-   **Wi-Fi Profile Management**: Backup and transfer saved Wi-Fi passwords by exporting them to XML files.
-   **Automated Updates**: Integrates Velopack for automatic checking and application of updates, keeping the tool current.

## Technical Stack
-   **Framework**: .NET 8.0
-   **UI**: Windows Presentation Foundation (WPF)
-   **Architecture**: Model-View-ViewModel (MVVM)
-   **Core Libraries**:
    -   **AlphaVSS**: For interaction with the Windows Volume Shadow Copy Service.
    -   **Velopack**: For application packaging, distribution, and auto-updates.
    -   **Caliburn.Micro**: A small framework for building powerful WPF applications.

## Getting Started

### Installation
The latest version of the MSP Technician toolkit can be downloaded from the [GitHub Releases](https://github.com/Red-Den-Software/MSP-Technician/releases) page. You can choose between the installer or the portable version.

### Building from Source
If you wish to build the application from the source code, follow these steps:

1.  **Prerequisites**:
    -   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

2.  **Clone the repository**:
    ```sh
    git clone https://github.com/Red-Den-Software/MSP-Technician.git
    cd MSP-Technician
    ```

3.  **Restore dependencies**:
    ```sh
    dotnet restore
    ```

4.  **Build the application**:
    ```sh
    dotnet build -c Release
    ```
The output will be located in the `bin/Release/net8.0-windows` directory.

## License
This project is licensed under the GNU General Public License v3.0. For more details, see the [LICENSE.txt](./LICENSE.txt) file.
