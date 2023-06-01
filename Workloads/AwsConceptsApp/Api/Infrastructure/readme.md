# Domain and Entity Relationship

|Entity            |Domain   |pk prefix|PK-Id      |skPrefix|SK-Id           |PK-Result          |SK-Result         |
|------------------|---------|---------|-----------|--------|----------------|-------------------|------------------|
|UserProfile       |U        |#        |userId     |-E#     |email           |U#userId           |U-E#email         |
|Organization      |O        |#        |orgId      |#       |orgId           |O#orgId            |O#orgId           |
|Person            |O_P      |#        |personId   |-O#     |orgId           |O_P#personId       |O_P-O#orgId       |
|PersonEmailMapping|O_P_E    |#        |emailId    |-O#     |orgId           |O_P_E#emailId      |O_P_E-O#orgId     |
|Dojo              |O_D      |#        |dojoId     |-O#     |orgId           |O_D#dojoId         |O_D-O#orgId       |
|DojoStaff         |O_DS     |-P#      |personId   |-O_D#   |orgId-dojoId    |O_DS-P#personId    |O_DS-O_D#dojoId   |
|DojoPupil         |O_DP     |-P#      |personId   |-O_D#   |orgId-dojoId    |O_DP-P#personId    |O_DP-O_D#dojoId   |
|Event             |O_E      |#        |eventId    |-O#     |orgId           |O_E#eventId        |O_E-O#orgId       |
|OffsiteEvent      |O_E_OS   |-E#      |eventId    |-O#     |orgId           |O_E_OS-E#eventId   |O_E_OS-O#orgId    |
|OffsiteEventCabins|O_E_OEC  |#        |           |        |                |O_E#               |O_E_              |



## Introduction
In this document, we will describe the relationship between different entities and their corresponding domains in a system. The entities and their attributes are listed in a table, and the relationship between them can be inferred from the naming convention used in the table.

## Domain
A domain represents a specific area of the system, and it is represented by a single letter or combination of letters. In the table, the domain is listed in the "Domain" column.

## Entity
An entity represents a specific object or concept within the system, and it is listed in the "Entity" column. Each entity belongs to a specific domain, and the relationship between the entity and its domain can be inferred from the naming convention used in the table.

## Primary Key
A primary key is a unique identifier for an entity, and it is used to identify and retrieve data from the database. In the table, the primary key is listed in the "PK-Id" column, and the prefix for the primary key is listed in the "pk prefix" column.

## Secondary Key
A secondary key is an additional identifier for an entity, and it is used to retrieve data from the database in addition to the primary key. In the table, the secondary key is listed in the "SK-Id" column, and the prefix for the secondary key is listed in the "skPrefix" column.

## PK-Result and SK-Result
The PK-Result and SK-Result columns list the final primary key and secondary key that are used in the database. These keys are formed by combining the prefix, the primary key and secondary key.

## Domain Hierarchy
The "_" used in the name of the entity represents the hierarchy of the domain. For example, the "Person" entity belongs to the "O_P" domain, which is a child domain of "O" (Organization). Similarly, "Dojo" entity belongs to the "O_D" domain and is a child domain of "O" (Organization).

## Conclusion  
This table illustrates the domain and entity relationship, primary key, and secondary key. The naming convention used in the table makes it easy to understand the relationship between different entities and how data is organized within the overall domain. It also makes it easy to understand the hierarchy of the domains and the relationship between parent and child domains. Overall this table provides a clear understanding of the structure of the data and the relationships between different entities in the system, which is essential for efficiently storing, retrieving and managing data in a database.