---
type: documentation
category: domain-adaptation
name: Education Domain Adaptation
status: active
---

# Education Domain Adaptation

This document explains how to adapt the generic vault to the education domain.

The goal is to help students convert lecture notes, textbooks, assignments, and exam preparation into a structured learning graph.

## Education-domain purpose

```text
Raw lecture notes
   ↓
Concept notes
   ↓
Prerequisite links
   ↓
Examples and practice questions
   ↓
Revision map
   ↓
Better learning
```

## Suggested education folders

You can adapt `10-Knowledge` like this:

```text
10-Knowledge/
  Concepts/
  Topics/
  Courses/
  Learning-Outcomes/
  Assignments/
  Assessments/
  Examples/
  Practice-Questions/
  Revision-Maps/
```

## Suggested note types

```text
course
topic
concept
definition
learning-outcome
example
assignment
assessment
practice-question
revision-note
mistake-note
```

## Suggested relationships

```text
includes
requires
explains
practiced_by
assessed_by
validates
belongs_to
part_of
derived_from
compared_with
```

## Example relationships

```markdown
- [[Normalization]] --requires--> [[Primary Key]]
- [[Foreign Key]] --requires--> [[Primary Key]]
- [[Assignment 01]] --practices--> [[SQL Joins]]
- [[Final Exam]] --assesses--> [[Database Design]]
- [[Practice Question 05]] --validates--> [[Normalization]]
```

## Education workflow

```text
1. Add lecture note to 00-Inbox
2. Extract concepts
3. Create concept notes
4. Link prerequisites
5. Add examples
6. Generate practice questions
7. Update revision map
8. Review weak concepts
```

## What a student can ask

- What should I learn before this topic?
- Which concepts are connected?
- What examples should I practice?
- Which topics are weak?
- What questions can come in the exam?
- How should I revise this course?

## Key principle for education

Do not only store notes.  
Turn notes into a learning graph.
