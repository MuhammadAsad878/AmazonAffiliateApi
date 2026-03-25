# Amazon Affiliate API (.NET Core)

A lightweight, secure Web API proxy built with .NET Core. This service handles requests from the React frontend, securely queries the RapidAPI Amazon Data Scraper, injects the designated Amazon Affiliate tag into product URLs, and returns clean JSON data.

## Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or newer)
* Visual Studio, VS Code, or JetBrains Rider

## Setup & Installation

1. **Clone the repository:**
   \`\`\`bash
   git clone <your-repo-url>
   cd AmazonAffiliateApi
   \`\`\`

2. **Restore dependencies:**
   \`\`\`bash
   dotnet restore
   \`\`\`

3. **Configure Environment Secrets:**
   Security best practices dictate that API keys are not committed to version control. You must create or update your local `appsettings.json` (or `appsettings.Development.json`) in the root directory with your RapidAPI credentials and Affiliate tag.

   Create the file and add the following structure:
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "RapidApi": {
       "Key": "YOUR_RAPIDAPI_KEY_HERE",
       "Host": "real-time-amazon-data.p.rapidapi.com"
     },
     "Amazon": {
       "AffiliateTag": "paksahulat-20"
     }
   }
   ```

## Running the Application

**Using the .NET CLI:**
\`\`\`bash
dotnet run
\`\`\`

**Using Visual Studio:**
Press the green "Play" button (IIS Express or https profile). 

*Note: As this is a pure Web API, navigating to the root `localhost` URL in the browser will return a 404. To test the API directly, navigate to the search endpoint.*

## Testing the Endpoint

Open your browser or Postman and navigate to:
\`\`\`text
https://localhost:<YOUR_PORT>/api/search?query=laptop
\`\`\`
*(Replace `<YOUR_PORT>` with the port assigned by your local environment, typically found in the terminal output or `Properties/launchSettings.json`).*
