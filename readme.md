## About

`AuthServer` is a basic implementation of Authorization and Authentication using JWTs (JSON Web Token), created as a demonstration for a lunch presentation to Epicodus students.

The desire was to show case creating something a little more involved than typical curriculum projects, and to demonstrate the importance of creating code that is maintainable, readable, and testable. 

In this project that is achieved by abstracting classes with interfaces and using dependency injection.

## How to use
1. First you will need to clone this repository to a directory of your choice.
`git clone https://github.com/mlstroud/AuthServer.git`

2. Update the connection string in `appsettings.development.json` for your database.
3. Create the following database tables in your preferred database(code originally used SqlServer, but MySql and others will work with modifications):
```
Users
	- Id (primary key, non null, auto increment)
	- Username (varchar(50), unique, non null)
	- Password (varchar(150), non null)
	- Salt (varchar(150), non null)
	- UserType (int, non null)

Claims
	- Id (primary key, non null, auto incremennt)
	- Type (varchar(50), non null)
	- Value (varchar(50), non null)
	- UserType (int, non null)

Create a unique index for the combination of (Type, Value, UserType).

UserClaims
	- Id (primary key, non null, auto increment)
	- UserId (foreign key referencing Users.Id, non null)
	- ClaimId (foreign key referencing Claims.Id, non null)

Create a unique index for the combination of (UserId, ClaimId).
``` 
4. Create the following stored procedures (some of these could be could practice for your SQL skills as you will need to use JOIN for the Claim related procedures.
```
Insert_User (should insert user data into the Users table),
Get_UserId (should return an Id from the Users table based off of username),
Get_User (should return all columns for a User based off of their username),
Assign_Claims (should assign claims to a user based off of their user type),
Get_Claims (should return all claims for a User based off of their user id)
```

5. Update stored procedure names in `UserRepository` and `ClaimRepository` to match the names  and/or schemas you created in step 4.
6. If you  are using MySql or another database provider besides SqlServer, you will need to modify the data access code.
```
Remove the nuget pacakge for System.Data.SqlClient.
Add the nuget package for whatever database provider you are using.
 - For example, MySql uses MySql.Data.
 
Remove references for System.Data.SqlClient and add the reference
for your new nuget package in DbHelper.cs and SprocExtensions.cs

Both of these files will be in the AuthServer.Library.DataAccess namespace.

For DbHelper.cs, update the method to return the new type of connection for your
corresponding database instead of the current SqlConnection being used.
```
7. Run the application.
8. To register an account, use Postman and send a `POST` request to `https://localhost:44312/api/auth/register`. If it is not running on that port, look in your AuthServer.Properties/launchSettings.json for the `sslPort` under `iisSettings:iisExpress`, and update the postman uri with the new port number.  Your request should look be formatted as follows (data type must be selected as raw -> JSON):
```
{
	"Username": "Username here",
	"Password": "Password here"
}
```

9. To login, submit a `POST` request to `https://localhost:44312/api/auth/login` with the post body below. The above steps for updating port and setting the data type apply here as well.
 ```
{
	"Username": "Username here",
	"Password": "Password here"
}
```

10. You should receive a `200` response containing an access token. To use it, open Postman and under `Authorization`, choose `Bearer` as the type, and paste your token before submitting the request. Submit a `GET` request to `https://localhost:44312/api/data` (additional endpoints include `/member` and `/admin`, appended after the `/api/data`. If you receive a `200` response for the plain `/api/data` endpoint, everything is working correctly (`/member` should work as well by default).

## Troubleshooting

For any errors that may occur during this process, please refer to the stack trace and error provided by your IDE to help diagnose issues.

If the program is otherwise executing fine but you are receiving unexpected responses from the endpoints (such as `401` or `500`), please refer to the logs at `AuthServer/Logs/` in your project directories. The application logs errors for common things that could fail and will help you locate the issue.

## Extra Credit

Complete the unit tests for the remainder of the `Service` classes (`UserService`, `TokenService`, `CryptographyService`, and `ClaimService`, using the existing test and what you learned in the presentation as a guide.

Expand or alter current functionality.

Integrate with another application you have developed, using the JWT provided by `AuthServer` to allow access to restricted endpoints in your application.