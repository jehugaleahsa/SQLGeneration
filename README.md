## This FORK 

This (hopefully temporary) fork of SQLGeneration contains some additional functionality not (yet) in core SQLGeneration.

### Batching 
You can parse (and build) batches of SQL statements, you are not limited to just a single statement. Individual statements in the batch must be terminated with a semicolon. Example:

```cs
    string commandText = "SELECT FirstName FROM Customer; INSERT INTO Names (Name) Values ('Chuck Norris');";
    CommandBuilder commandBuilder = new CommandBuilder();
    var builder = commandBuilder.GetCommand(commandText);
    
    Formatter formatter = new Formatter();
    commandText = formatter.GetCommandText(builder);
```

### DDL
I have added support for some DDL, defined below.

#### Create Database

Example of supported SQL: 
```
CREATE DATABASE MyDatabase

```

#### Create Table

Example of supported SQL: 
```
CREATE TABLE [dbo].[NewTable]
(
FavouriteLetter CHAR, -- simple column --
Reason VARCHAR(150), -- data type with size argument --
Balance DECIMAL(10,2), -- data type with scale and precision arguments -- 
Essay VARCHAR(MAX), -- datatype with 'max' argument -- 
ProfileText NTEXT COLLATE Latin1_General, -- collation -- 
MustPickAChar NCHAR NOT NULL, -- nullability, not null -- 
CanPickAChar NCHAR NULL, -- nullability, null -- 
AccountTypeName NVARCHAR DEFAULT 'Hello!', -- default string literal value -- 
Points INT DEFAULT 1, -- default numeric literal value -- 
SomeMessage NVARCHAR CONSTRAINT my_constraintname DEFAULT 'Wham!', -- default constraint, named -- 
RefNumber INT IDENTITY,  -- identity, no seed --
JobNumber INT IDENTITY(1,1), -- identity, with seed --
SomeId INT PRIMARY KEY, -- primary key constraint -- 
RegId INT CONSTRAINT my_primarykey PRIMARY KEY, -- primary key constraint, named (you wouldnt have 2 pk's though!) -- 
SpecialRefNumber INT UNIQUE, -- unique constraint  -- 
AccountRefNumber INT CONSTRAINT my_unique UNIQUE, -- unique constraint, named -- 
CustomerId INT NOT NULL FOREIGN KEY REFERENCES dbo.mytable, -- foreign key constraint, simple --
AccountId INT NOT NULL FOREIGN KEY REFERENCES dbo.mytable(accountid), -- foreign key constraint, with column ref --
SalaryId INT NOT NULL FOREIGN KEY REFERENCES salary(accountid) ON DELETE NO ACTION, -- foreign key constraint, with delete action --
DestinyId INT NOT NULL FOREIGN KEY REFERENCES destiny ON UPDATE CASCADE, -- foreign key constraint, with update action --
JeopardyId INT NOT NULL FOREIGN KEY REFERENCES jeopardy(id) ON DELETE SET NULL ON UPDATE SET DEFAULT, -- foreign key constraint, with delete action and update action --
ComplexExampleMessage VARCHAR(max) COLLATE Latin1_General NOT NULL DEFAULT 'Hello!', -- multiple of the above -- 
ComplexExampleOther INT NOT NULL CONSTRAINT my_constraintname DEFAULT 1 CONSTRAINT my_unique UNIQUE, -- multiple of the above -- 
ComplexManyConstraints INT NOT NULL CONSTRAINT my_default DEFAULT 1 CONSTRAINT my_fk FOREIGN KEY REFERENCES dbo.mytable(idColumn) ON DELETE SET NULL ON UPDATE NO ACTION CONSTRAINT my_unique UNIQUE
)
```

##### Supported Create Table Syntax (T-SQL)
The syntax highlighted in bold is supported, everything else is not yet supported.

<pre>
--Disk-Based CREATE TABLE Syntax
<strong>CREATE TABLE 
    [ database_name . [ schema_name ] . | schema_name . ] table_name</strong> 
    [ AS FileTable ]
    <strong>( { &lt;column_definition&gt;</strong> | &lt;computed_column_definition&gt; 
        | &lt;column_set_definition&gt; | [ &lt;table_constraint&gt; ] 
| [ &lt;table_index&gt; ] [ ,...n ] } )
    [ ON { partition_scheme_name ( partition_column_name ) | filegroup 
        | &quot;default&quot; } ] 
    [ { TEXTIMAGE_ON { filegroup | &quot;default&quot; } ] 
    [ FILESTREAM_ON { partition_scheme_name | filegroup 
        | &quot;default&quot; } ]
    [ WITH ( &lt;table_option&gt; [ ,...n ] ) ]
[ ; ]

&lt;column_definition&gt; ::= 
<strong>column_name &lt;data_type&gt;</strong>
    [ FILESTREAM ]
    <strong>[ COLLATE collation_name ] </strong>
    [ SPARSE ]
    <strong>[ NULL | NOT NULL ]</strong>
   <strong> [ 
        [ CONSTRAINT constraint_name ] DEFAULT constant_expression ] 
      | [ IDENTITY [ ( seed,increment ) ] [ NOT FOR REPLICATION ] 
    ]</strong>
   <strong> [ ROWGUIDCOL ]</strong>
   <strong> [ &lt;column_constraint&gt; [ ...n ] ] </strong>
    [ &lt;column_index&gt; ]

&lt;data type&gt; ::= 
<strong>[ type_schema_name . ] type_name 
    [ ( precision [ , scale ] | max | </strong>
        [ { CONTENT | DOCUMENT } ] xml_schema_collection ) ] 

&lt;column_constraint&gt; ::= 
<strong>[ CONSTRAINT constraint_name ] 
{     { PRIMARY KEY | UNIQUE } </strong>
        [ CLUSTERED | NONCLUSTERED ] 
        [ 
            WITH FILLFACTOR = fillfactor  
          | WITH ( &lt; index_option &gt; [ , ...n ] ) 
        ] 
        [ ON { partition_scheme_name ( partition_column_name ) 
            | filegroup | &quot;default&quot; } ]

  <strong>| [ FOREIGN KEY ] 
        REFERENCES [ schema_name . ] referenced_table_name [ ( ref_column ) ] 
        [ ON DELETE { NO ACTION | CASCADE | SET NULL | SET DEFAULT } ] 
        [ ON UPDATE { NO ACTION | CASCADE | SET NULL | SET DEFAULT } ] 
        [ NOT FOR REPLICATION ] </strong>

  <strong>| CHECK [ NOT FOR REPLICATION ]</strong> ( logical_expression ) 
} 

&lt;column_index&gt; ::= 
 INDEX index_name [ CLUSTERED | NONCLUSTERED ]
    [ WITH ( &lt;index_option&gt; [ ,... n ] ) ]
    [ ON { partition_scheme_name (column_name ) 
         | filegroup_name
         | default 
         }
    ] 
    [ FILESTREAM_ON { filestream_filegroup_name | partition_scheme_name | &quot;NULL&quot; } ]

&lt;computed_column_definition&gt; ::= 
column_name AS computed_column_expression 
[ PERSISTED [ NOT NULL ] ]
[ 
    [ CONSTRAINT constraint_name ]
    { PRIMARY KEY | UNIQUE }
        [ CLUSTERED | NONCLUSTERED ]
        [ 
            WITH FILLFACTOR = fillfactor 
          | WITH ( &lt;index_option&gt; [ , ...n ] )
        ]
        [ ON { partition_scheme_name ( partition_column_name ) 
        | filegroup | &quot;default&quot; } ]

    | [ FOREIGN KEY ] 
        REFERENCES referenced_table_name [ ( ref_column ) ] 
        [ ON DELETE { NO ACTION | CASCADE } ] 
        [ ON UPDATE { NO ACTION } ] 
        [ NOT FOR REPLICATION ] 

    | CHECK [ NOT FOR REPLICATION ] ( logical_expression ) 
] 

&lt;column_set_definition&gt; ::= 
column_set_name XML COLUMN_SET FOR ALL_SPARSE_COLUMNS

&lt; table_constraint &gt; ::=
[ CONSTRAINT constraint_name ] 
{ 
    { PRIMARY KEY | UNIQUE } 
        [ CLUSTERED | NONCLUSTERED ] 
        (column [ ASC | DESC ] [ ,...n ] ) 
        [ 
            WITH FILLFACTOR = fillfactor 
           |WITH ( &lt;index_option&gt; [ , ...n ] ) 
        ]
        [ ON { partition_scheme_name (partition_column_name)
            | filegroup | &quot;default&quot; } ] 
    | FOREIGN KEY 
        ( column [ ,...n ] ) 
        REFERENCES referenced_table_name [ ( ref_column [ ,...n ] ) ] 
        [ ON DELETE { NO ACTION | CASCADE | SET NULL | SET DEFAULT } ] 
        [ ON UPDATE { NO ACTION | CASCADE | SET NULL | SET DEFAULT } ] 
        [ NOT FOR REPLICATION ] 
    | CHECK [ NOT FOR REPLICATION ] ( logical_expression ) 

&lt; table_index &gt; ::= 
INDEX index_name [ CLUSTERED | NONCLUSTERED ] (column [ ASC | DESC ] [ ,... n ] ) 
    
    [ WITH ( &lt;index_option&gt; [ ,... n ] ) ] 
    [ ON { partition_scheme_name (column_name ) 
         | filegroup_name
         | default 
         }
    ] 
    [ FILESTREAM_ON { filestream_filegroup_name | partition_scheme_name | &quot;NULL&quot; } ]

} 
&lt;table_option&gt; ::=
{
    [DATA_COMPRESSION = { NONE | ROW | PAGE }
      [ ON PARTITIONS ( { &lt;partition_number_expression&gt; | &lt;range&gt; } 
      [ , ...n ] ) ]]
    [ FILETABLE_DIRECTORY = &lt;directory_name&gt; ] 
    [ FILETABLE_COLLATE_FILENAME = { &lt;collation_name&gt; | database_default } ]
    [ FILETABLE_PRIMARY_KEY_CONSTRAINT_NAME = &lt;constraint_name&gt; ]
    [ FILETABLE_STREAMID_UNIQUE_CONSTRAINT_NAME = &lt;constraint_name&gt; ]
    [ FILETABLE_FULLPATH_UNIQUE_CONSTRAINT_NAME = &lt;constraint_name&gt; ]
}

&lt;index_option&gt; ::=
{ 
    PAD_INDEX = { ON | OFF } 
  | FILLFACTOR = fillfactor 
  | IGNORE_DUP_KEY = { ON | OFF } 
  | STATISTICS_NORECOMPUTE = { ON | OFF } 
  | ALLOW_ROW_LOCKS = { ON | OFF} 
  | ALLOW_PAGE_LOCKS ={ ON | OFF} 
  | DATA_COMPRESSION = { NONE | ROW | PAGE }
       [ ON PARTITIONS ( { &lt;partition_number_expression&gt; | &lt;range&gt; } 
       [ , ...n ] ) ]
}
&lt;range&gt; ::= 
&lt;partition_number_expression&gt; TO &lt;partition_number_expression&gt;
</pre>

#### Alter Table

Example of supported SQL: 
```
ALTER TABLE MyTable ADD mycolumn VARCHAR(100) NOT NULL, myothercolumn INT NOT NULL PRIMARY KEY;
ALTER TABLE MyTable ALTER COLUMN mycolumn VARCHAR(10) COLLATE latin_general NOT NULL;
ALTER TABLE MyTable ALTER COLUMN mycolumn DROP ROWGUIDCOL;
ALTER TABLE MyTable ALTER COLUMN mycolumn ADD NOT FOR REPLICATION;
ALTER TABLE MyTable DROP CONSTRAINT myconstraint, myotherconstraint, COLUMN andacolumn, andanothercolumn, CONSTRAINT finalconstraint, COLUMN finalcolumn;
```

##### Supported Alter Table Syntax (T-SQL)
The syntax highlighted in bold is supported, everything else is not yet supported.

<pre>

<strong>ALTER TABLE [ database_name . [ schema_name ] . | schema_name . ] table_name 
{ 
    ALTER COLUMN column_name 
    { 
        [ type_schema_name. ] type_name 
            [ ( 
                { 
                   precision [ , scale ] 
                 | max </strong>
                 | xml_schema_collection 
                <strong>} 
            ) ] 
        [ COLLATE collation_name ] 
        [ NULL | NOT NULL ]</strong> [ SPARSE ]
      <strong>| {ADD | DROP } 
        { ROWGUIDCOL | PERSISTED | NOT FOR REPLICATION | SPARSE }</strong>
   <strong> }</strong> 
        | [ WITH { CHECK | NOCHECK } ]

    <strong>| ADD 
    { 
        &lt;column_definition&gt;</strong>
      | &lt;computed_column_definition&gt;
      | &lt;table_constraint&gt; 
      | &lt;column_set_definition&gt; 
    <strong>} [ ,...n ]</strong>

   <strong> | DROP 
     {
         [ CONSTRAINT ] 
         { 
              constraint_name </strong>
              [ WITH 
               ( &lt;drop_clustered_constraint_option&gt; [ ,...n ] ) 
              ] 
          <strong>} [ ,...n ]
          | COLUMN 
          {
              column_name 
          } [ ,...n ]
     } [ ,...n ]</strong>
    | [ WITH { CHECK | NOCHECK } ] { CHECK | NOCHECK } CONSTRAINT 
        { ALL | constraint_name [ ,...n ] } 

    | { ENABLE | DISABLE } TRIGGER 
        { ALL | trigger_name [ ,...n ] }

    | { ENABLE | DISABLE } CHANGE_TRACKING 
        [ WITH ( TRACK_COLUMNS_UPDATED = { ON | OFF } ) ]

    | SWITCH [ PARTITION source_partition_number_expression ]
        TO target_table 
        [ PARTITION target_partition_number_expression ]
        [ WITH ( &lt;low_lock_priority_wait&gt; ) ]
    | SET ( FILESTREAM_ON = 
            { partition_scheme_name | filegroup | &quot;default&quot; | &quot;NULL&quot; } 
          )

    | REBUILD 
      [ [PARTITION = ALL]
        [ WITH ( &lt;rebuild_option&gt; [ ,...n ] ) ] 
      | [ PARTITION = partition_number 
           [ WITH ( &lt;single_partition_rebuild_option&gt; [ ,...n ] ) ]
        ]
      ]

    | &lt;table_option&gt;

    | &lt;filetable_option&gt;

}
[ ; ]

</pre>


# SQLGeneration

Provides core classes for generating SQL at runtime.

Download using NuGet: [SQLGeneration](http://nuget.org/packages/SQLGeneration)

## Recent Updates
Please view the [Recent Updates Wiki](https://github.com/jehugaleahsa/SQLGeneration/wiki/Recent-Updates) to learn the latest news.

## Overview
With all the ORMs out there today, it's amazing how many systems have their own micro-ORMs. We've all been there. We start writing SQL by hand and using core ADO.NET classes. The better of us try to use parameters whenever possible; others just concatenate strings. Even those virtuous enough to stick with parameters eventually run into problems... an IN filter, for example, or a screen that allows users to sort and filter columns.

At this point even really experienced programmers start to sweat. "I can build my IN filter, because all the values are integers... right?", or "I just have to build my WHERE and ORDER BY clauses, I just have to remember to check for zero items first..." This is where the trouble starts for a lot of projects. It's very easy to introduce a bug that makes your code vulnerable to malicious users, especially if you have entry-level developers working on your data layer. On a much less serious note, most developers forget simple things like handling empty lists, escaping strings, handling NULL values and making sure there's whitespace where it's needed.

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
    
As you can see from this example, generating commands requires a lot of typing. Most of the time, this type of code would be replaced with an automated process. For instance, you might loop through `DataColumn`s in a `DataTable` or use reflection to visit `Attribute`s in a class to configure the builder.

Note the use of the `Placeholder` class in the example. `Placeholder` allows you to place arbitrary values in the SQL statement. You could use this, for instance, to create an number with an exponent (`new Placeholder("6.02214078E23")`). It's more common use is to indicate parameters, like in the example. It's really a shortcut when you want to avoid the object model. Just keep in mind that invalid values will cause errors.

## SELECT for the Lazy and Arthritic
If you had to do that much typing for every SELECT statement, you'd probably just stick to concatenating strings. Not everyone is building an ORM, so I made it possible to build command builders from plain SQL. Here's the same example, simplified:

    string commandText = "SELECT CustomerId, FirstName, LastName, Created FROM Customer";
    CommandBuilder commandBuilder = new CommandBuilder();
    SelectBuilder builder = (SelectBuilder)commandBuilder.GetCommand(commandText);
    
    Column customerId = builder.Source["Customer"].Column("CustomerId");
    Placeholder parameter = new Placeholder("@CustomerId");
    EqualToFilter filter = new EqualToFilter(customerId, parameter);
    builder.AddWhere(filter);
    
    Formatter formatter = new Formatter();
    commandText = formatter.GetCommandText(builder);

## License
If you are looking for a license, you won't find one. The software in this project is free, as in "free as air". Feel free to use my software anyway you like. Use it to build up your evil war machine, swindle old people out of their social security or crush the souls of the innocent.

I love to hear how people are using my code, so drop me a line. Feel free to contribute any enhancements or documentation you may come up with, but don't feel obligated. I just hope this code makes someone's life just a little bit easier.
