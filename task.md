# Reporting Service

Imagine you are asked to fix a reporting service previously maintained by a developer who has since left the company.  
All you know is that the service is supposed to generate an **accounting report for a selected month** for all employees in the organization.  
There is no other functionality in this reporting service.

---

## Example Report

**January 2017**

---

### Finance Department

John Smith — €3,500  
Robert Johnson — €3,250  
Michael Williams — €4,000  
David Brown — €4,500  

**Total for department:** €15,250

---

### Accounting

William Davis — €2,500  
Thomas Miller — €2,750  
Charles Wilson — €1,750  

**Total for department:** €7,000

---

### IT Department

James Anderson — €4,500  
Daniel Thompson — €6,000  
Christopher Martinez — €5,500  
Andrew Taylor — €6,000  

**Total for department:** €22,000

---

**Total for company:** €44,250

---

## Problem Description

The reporting service is already deployed in production, but it is extremely unstable. At some point it stopped working entirely.  
Your task is to bring the reporting service back to life.

The only information available about the internal design of the service is contained in a note left by the previous developer:

> The list of employees by department can be retrieved from the `employee` database.  
> An employee’s monthly salary can be obtained from the Accounting Department’s web service,  
> but it requires the employee code provided by the HR system.

User-reported bugs (from the time when the service still partially worked):

+ Sometimes the system does not return a report and instead throws an error  
+ The service is extremely slow  
+ Not all employees appear in the report  
+ The “total for company” line is missing  

Your job is to restore the reporting service to working condition, fix the known bugs, and clean up the project, since the previous developer was not particularly organized.

You **do not** have access to the database or the web services, because the data they contain is strictly confidential.  
And yes — we’ve heard something about tests. It would be great to cover the code with tests so we don’t end up in such a situation again.
