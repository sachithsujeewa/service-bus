---
type: use-case-seed
id: ASUC-08
name: Control DB Outage
status: seed
chapter: 11-New-System
---

# ASUC-08: Control Database Outage

## Intent

Specify behavior when RambaseServiceBus SQL unavailable while ERP and broker healthy.

## As-is

Hosts prefer passive → **cluster-wide webhook stop** ([[CONC-003]]).

## Target options

1. HA SQL cluster for control DB
2. Decouple leadership from SQL (K8s lease)
3. Degraded mode: delivery continues with documented risk

## Acceptance

Explicit RTO/RPO for control plane vs data plane.

## Relationships

- [[uc-control-db-outage]] --explained_by--> [[ha-dr-concerns]]
