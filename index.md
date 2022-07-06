# Project references


### Database design and development

At the University I designed a database based on custom theme. I created the Entity Relation diagram and the database scheme. I created the specified tables and filled them with random generated data. I wrote various sql queries, stored procedures, functions, triggers in accordance with the course requirements (simple queries, grouping, subqueries, etc.). My solution is documented [here](https://github.com/BolykiAgnes/bolykiagnes.github.io/tree/main/database).


### ASP.NET MVC
I used the ASP.NET MVC framework in a webshop-like application where people can offer their own products for sale. The use of the webshop requires authorization and authentication. There are 3 user roles: admin, student and normal user. The admin has full control on the website, the students have discount on every product. The users can post an ad with picture and description. The project is available [here](https://github.com/BolykiAgnes/bolykiagnes.github.io/tree/main/ASP.NET%20MVC/Hardverapro).


### Thesis work
My thesis work was the development of a smart parking system. I researched the literature with similar systems, specified the operation process of the system, the tasks to be satisfied. I created the system design, chose the frameworks, tools and softwares for the implementation. After the development, I tested my system's functionalities. The system consists of a web application where the users can register, reserve a spot in a parking place, view and manage their reservations; and an on-site system which task is to allow or deny the arriving cars to enter the parking place and monitor the occupancy of the parking spots. For the web application's database I used the SQLite database engine, the API was created in ASP.NET Core framework with the use of C# language and the frontend was developed in Angular with TypeScript. The on-site system has a license plate recognition API, which receives a camera image, does the license plate recognition process and sends the result to my API where the business logic determines whether the user is entitled to enter. In the parking place every spot has an ultrasound sensor to sense the presence of the car, it is connected to a NodeMCU-ESP8266 microcontroller which sends the changes of the occupancy statuses to my API.
The documentation and source code available [here](https://github.com/BolykiAgnes/bolykiagnes.github.io/tree/main/Thesis%20Work).
