# FacebookAPI (C# edition)

## Prerequisites
1. Downlaod or clone this project to your desired location.
2. If you have done so already, download and install XAMPP at https://www.apachefriends.org/
3. Create a database called "facebook" (case-sensitive)
4. Create a file in the root directory of the DataAccess project called "SecretConfig.cs" (case-sensitive).
5. Copy and paste the following code:
```c#
namespace FacebookAPI_CSharp.DataAccess
{
    public class SecretConfig
    {
        public static string ConnectionString { get; set; } = "server=yourserver;user=yourusername;password=yourpassword;database=facebook";
    }
}
```
