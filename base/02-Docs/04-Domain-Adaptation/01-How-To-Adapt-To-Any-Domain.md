---
type: documentation
category: domain-adaptation
name: How To Adapt To Any Domain
status: active
---

# How To Adapt To Any Domain

This vault starts generic.  
To adapt it to a domain, define the domain's knowledge types, relationships, templates, and workflows.

## Step 1: Define the domain

Example domains:

- Education
- Software engineering
- Healthcare
- Business operations
- Research
- Legal
- Manufacturing
- Product management

## Step 2: Identify domain objects

Ask:

```text
What are the main things in this domain?
```

Examples:

Education:

```text
Course
Topic
Concept
Learning Outcome
Assignment
Assessment
Student Note
Practice Question
```

Business:

```text
Customer
Product
Process
Policy
Risk
Decision
KPI
Report
```

## Step 3: Define note types

Example:

```text
concept
entity
process
rule
decision
example
question
assessment
map
```

## Step 4: Define relationships

Example:

```text
requires
includes
belongs_to
explains
validates
supports
uses
produces
depends_on
```

## Step 5: Create templates

Create templates for important knowledge types.

## Step 6: Add examples

Examples help users and agents follow the correct structure.

## Step 7: Add validation rules

Decide what must be reviewed by humans.

## Step 8: Maintain the graph

Keep maps and graph files updated as the domain grows.

## Simple formula

```text
Domain vocabulary
   +
Note templates
   +
Typed relationships
   +
Review rules
   =
Domain-adapted knowledge vault
```
