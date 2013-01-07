# SQLGeneration

Provides core classes for generating SQL at runtime.

Download using NuGet: [SQLGeneration](http://nuget.org/packages/SQLGeneration)

## Overview
Even though there are plenty of decent ORMs out there on the market (most for free), perhaps the day has come where you need to write your own SQL at runtime. Or, perhaps you're just an hacker trying to get fancy, writing your own custom data layer for your system. Whatever your reason for writing SQL at runtime, doing it well can be a tiresome affair. Half the battle is the SQL generation itself. Anything beyond a simple, flat SELECT statement can get pretty tricky.

That's where SQLGeneration comes in. It is an extremely simple library that makes building SQL at runtime easier. It has support for your basic CRUD operations, including variations of SELECT, INSERT, UPDATE and DELETE. It makes it easy to build WHERE clauses, multiple JOINs and complex expressions, like function calls and arithmetic.

SQLGeneration doesn't support everything. There are so many vendors with so many extensions to SQL that it would be impossible to capture them all. Still, SQLGeneration provides some options for the more popular vendors. SQLGeneration provides interfaces that you can implement and plug in when you need something that isn't provided. SQLGeneration is a pretty small library, too, so it isn't hard to modify for your own needs.

## Quick Samples
Someone first digging through SQLGeneration might be overwhelmed by the number of interfaces and classes. Most of these you can ignore unless you are customizing SQLGeneration's behavior. The classes you need to focus on are `SelectBuilder`, `InsertBuilder`, `UpdateBuilder` and `DeleteBuilder`.

Here's a "Hello World" example that runs on SQL Server:

    SelectBuilder builder = new SelectBuilder();
    builder.AddProjection(new StringLiteral("Hello, World!!!"));
    string commandText = builder.GetCommandText();  // Hello, World!!!
    
Here's the same example that runs on Oracle:

    SelectBuilder builder = new SelectBuilder();
    builder.AddProjection(new StringLiteral("Hello, World!!!"));
    builer.AddJoinItem(new Table("dual"));
    string commandText = builder.GetCommandText();  // Hello, World!!!
    
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
    PrefixParameterDecorator decorator = new PrefixParameterDecorator("@");
    Column orderDate = order.CreateColumn("OrderDate");
    Parameter orderDateParameter = new Parameter(decorator, "orderDate");
    IFilter filter = new GreaterThanFilter(orderDate, orderDateParameter);
    builder.Where.AddFilter(filter);
    
    // ORDER BY o.OrderDate DESC
    builder.AddOrderBy(new OrderBy(orderDate, Order.Descending));
