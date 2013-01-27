# SQLGeneration

Provides core classes for generating SQL at runtime.

Download using NuGet: [SQLGeneration](http://nuget.org/packages/SQLGeneration)

## Status Update
I am currently in the process of simplifying this library. The first part of that will be eliminating any unnecessary interfaces. For whatever reason, I created a lot of interfaces; however, the rest of the library didn't interface with those... interfaces, so, what's the point?

As another part of making things easier, I am trying to support custom SQL formatting. Initially, this project only generated boring SQL that wasn't very human-readable. Since most people will just be sending the output to a database engine, the output format isn't that significant. However, it might be useful to generate pretty SQL for debugging purposes or if the SQL will be copied into a stored procedure or if someone is writing a front-end that displays SQL.

With the latest check-in, I have included a rather dumb formatter that separates every token with a space. This will allow the SQL to be run against a database but isn't minified or human-readable. My goal with the next check in will be generating the minimal output again. Then, I will focus on writing a pretty SQL formatter that can be customized.

I am hoping to make custom formatting possible by generating an abstract syntax tree (AST) that a formatter can navigate. I need to provide enough information about the tokens and their relative position in the SQL to allow a formatter to make formatting decisions. I don't know the exact details yet, but I'm sure that will become more obvious as I work on it.

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
