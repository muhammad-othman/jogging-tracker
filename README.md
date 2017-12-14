
###### Jogging Tracker Info ########

###### Viewing the serverside
`http://localhost:4218`

###### Viewing the clientside
`http://localhost:58217`

# Schema

## Jogs

- id  (integer)
- date  (date)
- distance  (integer)
- duration  (integer)
- userID  (integer)
- userName  (string)



## User

- id  (integer)
- userName  (string)
- age  (integer)
- email  (string)
- permission  (integer)

#########################################################################################

# API Resources

## Jogs
```
GET|POST          /api/jogs
GET|PUT|DELETE    /api/jogs/{id}
GET 			  /api/jogs/{userID}      				//for Admins&Managers
GET 			  /api/jogs/{pageIndex}{pageSize}       //Server Side Pagination
GET 			  /api/jogs/{from}{to}       			//Date Filtering


## JogsReport
```
GET               /api/jogs/report/{userID?}


## Users
```
GET|POST          /api/users
GET|PUT|DELETE    /api/users/{id}
GET 			  /api/users/{pageIndex}{pageSize}      //Server Side Pagination

## Account
```
POST          	  /api/register
POST          	  /api/login
POST          	  /api/logout
GET          	  /api/userinfo
GET 		      /api/permissions						//for Admins&Managers

