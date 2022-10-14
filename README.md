# FacebookAPI (C# edition)

## Prerequisites
1. Downlaod or clone this project to your desired location.
2. If you have done so already, download and install XAMPP at https://www.apachefriends.org/
3. Create a database called "facebook" (case-sensitive)
4. Create a file in the root directory of the DataAccess project called "SecretConfig.cs" (case-sensitive).
5. Copy and paste the following code:
```c#
namespace FacebookAPI.DataAccess
{
    public class SecretConfig
    {
        public static string ConnectionString { get; } = "server=yourserver;user=yourusername;password=yourpassword;database=facebook";
    }
}
```
6. Create database
<br>
<b>Make sure the "Mysql" and "Apache" module in XAMPP is running</b>

### Visual Studio instructions
1. Change default project "FacebookAPI.DataAccess".
2. Run the following command in Package Manager Console
```console
Update-Database
```

### VSCode instructions
1. Open a termal with the root path.
2. Change your path to DataAccess folder.
```console
cd FacebookAPI-CSharp.DataAccess/
```
4. Run the following command:
```console
dotnet ef database update
```

## Usage
1. Press ctrl + f5
2. This is willl open a browser window.
3. Use the Swagger UI to make requests to the database. The "Mysql" and "Apache" modules in XAMPP must be running.