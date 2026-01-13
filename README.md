**DONE**
1. Unused nuget packages.
2. Using old netcore 2.1, update libs.
3. N+1 problem, querying all employees
4. All business logic in the controller.
5. Don't write a report to file, keep it as a stream.
6. Use HttpClientFactory instead of HttpClient directly. 
7. Documentation for the controller, entities. 
8. Cache for buhcode. 
9. Add a handling error middleware. 
10. Common logs. 
11. Do not keep the app configuration in classes.


**TODO**
1. Corner cases: a department has 0 employees, a report has 0 departments. 
2. Batching in case of performance issues. 
3. Why don't we filter by the date parameter?
4. Hangfire for not keeping http connection while generating a report and for observability.
5. CORS
6. Swagger
7. More logs, metrics, traces