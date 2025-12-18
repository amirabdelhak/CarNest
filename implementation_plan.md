# Cleanup Synchronous Repository Methods

## Goal Description
Remove unused synchronous methods from the Data Access Layer to enforce usage of Asynchronous patterns and clean up the codebase.

## User Review Required
> [!IMPORTANT]
> The following synchronous methods will be removed: `GetAll`, `GetById`, `Count`, `Any`, and `Save`. Verify that no other part of the application (e.g., legacy services or background jobs not in BLL) relies on them.

## Proposed Changes
### DAL Layer
#### [MODIFY] [IGenericRepository.cs](file:///d:/projects/CarNest/DAL/Repository/IGenericRepository.cs)
- Remove `GetAll()`, `GetById()`, `Count()`, `Any()` synchronous definitions.
- Keep `Add()`, `Update()`, `Delete()` as they are in-memory context operations.

#### [MODIFY] [GenericRepository.cs](file:///d:/projects/CarNest/DAL/Repository/GenericRepository.cs)
- Remove implementations of `GetAll()`, `GetById()`, `Count()`, `Any()`.

#### [MODIFY] [IUnitOfWork.cs](file:///d:/projects/CarNest/DAL/UnitOfWork/IUnitOfWork.cs)
- Remove `Save()` synchronous definition.

#### [MODIFY] [UnitOfWork.cs](file:///d:/projects/CarNest/DAL/UnitOfWork/UnitOfWork.cs)
- Remove `Save()` implementation.

## Verification Plan
### Automated Tests
- Run `dotnet build` to ensure no compilation errors (verifying no remaining usages).
### Manual Verification
- None required beyond compilation check as this is a cleanup task.
