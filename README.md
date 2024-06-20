# 3x-UI Brute Force Tool

The **3x-UI Brute Force Tool** is a command-line interface (CLI) application developed in C# to assist in the process of brute forcing login credentials for 3x-UI panels. This tool is intended for educational purposes only, allowing users to understand and explore the concepts of cybersecurity, including penetration testing and vulnerability assessment.

## Features

- **Multi-threaded Brute Force**: Utilizes multiple threads to increase the speed of the brute force attack.
- **Customizable Wordlists**: Supports the use of custom wordlists for usernames and passwords.
- **Detailed Logging**: Logs all attempts and successes, providing a detailed record of the brute force process.
- **Proxy Support**: Optionally integrates with proxies to anonymize the brute force attempts.

## Usage

1. **Clone the Repository**:
   ```sh
   git clone https://github.com/jonifon/3x-bruter.git
   cd 3x-ui-bruter
   ```
2. **Build the Project:**
   ```sh
   dotnet build
   ```
3. Run the Tool:
   ```sh
   dotnet run -- --help
   ```

## Disclaimer

This tool is provided for educational purposes only. The author does not condone or support the use of this tool for any illegal activities. Users are responsible for ensuring they have permission to test the security of the systems they are targeting. Unauthorized use of this tool may result in legal consequences.



## License
This project is licensed under the MIT License. See the LICENSE file for details.
