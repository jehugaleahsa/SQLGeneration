# SQLGeneration

Provides core classes for generating SQL at runtime.

Download using NuGet: [SQLGeneration](http://nuget.org/packages/SQLGeneration)

## Status Update
On 02/05/2013 I finished implementing a crude SQL parser and I finished implementing a simple formatter. I need to write some tests to make sure the parser is correctly interpreting different parts of the SQL. So far things are looking really good. Once I feel more confident in my parser (after lots of unit testing) I will release this new version to NuGet.

Then I will be focusing on going in the reverse... building command builders from SQL. This will make it possible to provide "almost there..." SQL that you can then modify as needed.

Then I will see if I can act upon a `DataSet` like I'd act upon a database using SQL. This will make it possible to treat a `DataSet` like any other database, making it an ideal for unit testing, prototyping or doing offline work. I've always hated that `DataSet`s didn't support a basic SQL language. Needing to call `Filter` or directly manipulate the rows was certainly unpleasant.

Then I'll concern myself with writing a Pretty SQL formatter. This will be useful for debugging or storing SQL permanently.

Finally, I would like to create a small wrapper library that makes it possible to build commands more naturally via a natural language syntax (method call chaining). When using this library to generate SQL as part of an automated process, having a monotonous, wordy object model is actually a good thing. But for people who want to use this library to avoid writing SQL altogether, it would be nice to do it in a way that doesn't cause permanent brain trauma.

## Overview
Even though there are plenty of decent ORMs out there on the market (most for free), perhaps the day has come where you need to write your own SQL at runtime. Or, perhaps you're just a hacker trying to get fancy, writing your own custom data layer for your system. Whatever your reason for writing SQL at runtime, doing it well can be a tiresome affair. Half the battle is the SQL generation itself. Anything beyond a simple, flat SELECT statement can get pretty tricky.

That's where SQLGeneration comes in. It is an extremely simple library that makes building SQL at runtime easier. It has support for your basic CRUD operations, including variations of SELECT, INSERT, UPDATE and DELETE. It makes it easy to build WHERE clauses, multiple JOINs and complex expressions, like function calls and arithmetic expressions.

SQLGeneration doesn't support everything. There are so many vendors with so many extensions to SQL that it would be impossible to capture them all. Still, SQLGeneration provides some options for the more popular vendors. SQLGeneration provides interfaces that you can implement and plug-in when you need something that isn't provided. SQLGeneration is a pretty small library, too, so it isn't hard to modify for your own needs.

## Quick Examples
Someone first digging through SQLGeneration might be overwhelmed by the number of interfaces and classes. Most of these you can ignore unless you are customizing SQLGeneration's behavior. The classes you need to focus on are `SelectBuilder`, `InsertBuilder`, `UpdateBuilder` and `DeleteBuilder`.

Here's a "Hello World" example that runs on SQL Server:

    SelectBuilder builder = new SelectBuilder();
    builder.AddProjection(new StringLiteral("Hello, World!!!"));
    string commandText = builder.GetCommandText();  // SELECT 'Hello, World!!!' 
    
Here's the same example that runs on Oracle:

    SelectBuilder builder = new SelectBuilder();
    builder.AddProjection(new StringLiteral("Hello, World!!!"));
    builer.AddJoinItem(new Table("dual"));
    string commandText = builder.GetCommandText();  // SELECT 'Hello, World!!!' FROM dual
    
Now, here's a more realistic example that simply grabs some records:

    // SELECT *
    SelectBuilder builder = new SelectBuilder();
    builder.AddProjection(new AllColumns());
    
    // FROM customer c INNER JOIN order o ON (c.CustomerId = o.CustomerId)
    Table customer = new Table("customer") { Alias = "c" };
    Table order = new Table("order") { Alias = "o" };
    Column primaryKey = customer.CreateColumn("CustomerId");
    Column foreignKey = order.CreateColumn("CustomerId");
    IJoin join = new InnerJoin(customer, order);
    join.AddFilter(new EqualToFilter(primaryKey, foreignKey));
    builder.AddJoinItem(join);
    
    // WHERE o.OrderDate > @orderDate
    Column orderDate = order.CreateColumn("OrderDate");
    Parameter orderDateParameter = new Parameter("@orderDate");
    IFilter filter = new GreaterThanFilter(orderDate, orderDateParameter);
    builder.Where.AddFilter(filter);
    
    // ORDER BY o.OrderDate DESC
    builder.AddOrderBy(new OrderBy(orderDate, Order.Descending));

As you can see, the length of the code can get quite long when dealing with realistic queries. However, this code also demonstrates where SQLGeneration is really useful. It will automatically prefix every reference to the defined columns with the table name (or alias, if provided).

Here's a quick example of inserting into a table:

    Table customer = new Table("customer");
    InList values = new InList();
    InsertBuilder builder = new InsertBuilder(customer, values);
    builder.AddColumn(customer.CreateColumn("FirstName"));
    builder.AddColumn(customer.CreateColumn("LastName"));
    values.AddValue(new StringLiteral("John"));
    values.AddValue(new StringLiteral("Doe"));
    string commandText = builder.GetCommandText();  // INSERT INTO customer (FirstName, LastName) VALUES('John', 'Doe')
    
You can also insert the results of a select statement:

    // INSERT INTO CustomerCount (Count)
    Table customerCount = new Table("CustomerCount");
    SelectBuilder values = new SelectBuilder();
    InsertBuilder builder = new InsertBuilder(customerCount, values);
    builder.AddColumn(customerCount.CreateColumn("Count"));
    
    // (SELECT COUNT(1) FROM Customer)
    Table customer = new Table("Customer");
    Function function = new Function("COUNT");
    function.Arguments.AddValue(new NumericLiteral(1));
    values.AddProjection(function);
    values.AddJoinItem(customer);
    
Updates follow a similar pattern:

    // UPDATE customer
    Table customer = new Table("customer");
    UpdateBuilder builder = new UpdateBuilder(customer);
    
    // SET FirstName = 'John', LastName = 'Doe'
    Column firstName = customer.CreateColumn("FirstName");
    Column lastName = customer.CreateColumn("LastName");
    builder.AddSetter(new Setter(firstName, new StringLiteral("John")));
    builder.AddSetter(new Setter(lastName, new StringLiteral("Doe")));
    
    // WHERE CustomerId = 123
    Column customerId = customer.CreateColumn("CustomerId");
    IFilter filter = new EqualToFilter(customerId, new NumericLiteral(123));
    builder.Where.AddFilter(filter);
    
Finally, here is an example doing a delete:

    Table customer = new Table("customer");
    DeleteBuilder builder = new DeleteBuilder(customer);
    Column customerId = customer.CreateColumn("CustomerId");
    IParameter parameter = new Parameter("@customerId");
    IFilter filter = new EqualToFilter(customerId, parameter);
    builder.Where.AddFilter(filter);
    string commandText = builder.GetCommandText();  // DELETE FROM customer WHERE CustomerId = @customerId
