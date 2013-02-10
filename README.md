# SQLGeneration

Provides core classes for generating SQL at runtime.

Download using NuGet: [SQLGeneration](http://nuget.org/packages/SQLGeneration)

## Overview
With all the ORMs out there today, it's amazing how many systems have their own micro-ORMs. We've all been there. We start writing SQL by hand and using core ADO.NET classes. Some of us try to use parameters whenever possible; others just concatenate strings. Even those virtuous enough to stick with parameters eventually run into problems... an IN filter, for example, or a screen that the user can sort and filter values on.

At this point, even really experienced programmers start to sweat. "I can build my IN filter, because all the values are integers... right?", or "I just have to build my WHERE and ORDER BY clauses, I think." This is where the trouble starts for a lot of projects. It's very easy to introduce a bug that makes your code vulnerable to malicious users, especially if you have entry level developers working on your data layer. On a much less serious note, most developers forget things like handling empty lists, escaping strings, handling NULL values and making sure there's whitespace where it's needed.

SQLGeneration is meant to alleviate some of the headache involved in building SQL in you code. It handles all of the formatting issues for you, so you can concentrate on what your SQL is supposed to be doing. Whether you're just handling that one troublesome SQL statement or you've decided to build your own micro-ORM, SQLGeneration will make your job easier.

It sports an extremely flexible object model that supports generating fairly standard SQL for different vendors. It doesn't support everything, of course, but it covers a lot of ground for everyday development, especially for SQL Server, Oracle, MySQL, PostgresSQL and even MS Access. Support for new features are added regularly.

## A Simple SELECT
Here is a simple example that selects some columns from a table:

    // SELECT CustomerId, FirstName, LastName, Created FROM Customer WHERE CustomerId = @CustomerId
    SelectBuilder builder = new SelectBuilder();
    
    AliasedSource customer = builder.AddTable(new Table("Customer"));
    
    Column customerId = customer.Column("CustomerId");
    Column firstName = customer.Column("FirstName");
    Column lastName = customer.Column("LastName");
    Column created = customer.Column("Created");
    builder.AddProjection(customerId);
    builder.AddProjection(firstName);
    builder.AddProjection(lastName);
    builder.AddProjection(created);
    
    Placeholder parameter = new Placeholder("@CustomerId");
    EqualToFilter filter = new EqualToFilter(customerId, parameter);
    builder.AddWhere(filter);
    
    Formatter formatter = new Formatter();
    string commandText = formatter.GetCommandText(builder);
    
As you can see from this example, generating commands requires a lot of typing. Most of the time, this type of code would be built as part of an automated process. For instance, you might loop through `DataColumn`s in a `DataTable` or use reflection to visit `Attribute`s in a class to configure the builder.

Note the use of the `Placeholder` class in the example. `Placeholder` allows you to place arbitrary values in the SQL statement. You could use this, for instance, to create an number with an exponent (`new Placeholder("6.02214078E23")`). It's more common use is to indicate parameters, like in the example. It's really a shortcut when you want to avoid the object model. Just keep in mind that invalid values will cause errors.
