Data Model: A set of classes (often called entity classes or Domain models) that represent the structure of the data your application deals with.

- EF Core uses these models to create or map to db schemas.
 
  
## For Docker

 ### Step: 1 pull from server
  `docker pull mcr.microsoft.com/mssql/server`

### Step: 2 Set Password
docker run -e 'ACCEPT_EULA==Y' -e 'SA_PASSWORD=StrongPassword@01'-p 1400:1433 -d mcr.microsoft.com/mssql/server

getting Image: 
`ce9f9d1cf1520e4b48d7013734764367d93eaeb8dcb1af3a230a7e24956b620c`


scafolding: scafolding is a code generation technique.

DTO: Data shaping.
 - Return only relevent data in Response.
 - Minimize payload for performance
 - Avoid leaking sensitive fields.
 - Achivie using mapping or projecting(e.g LINQ Select)

-  Optionally use a mapper to map between DTOs and domain models.

Next Course:
- solid and clean architecture course
- Entity framework course


