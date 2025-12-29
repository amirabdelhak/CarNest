# Implementation Plan - Car Approval System

## Goal Description
Implement a moderation system where cars posted by Vendors must be approved by an Admin before they appear on the public feed. Admin posts are auto-approved.

## User Review Required
> [!IMPORTANT]
> This change requires a database migration to add the `Status` column to the `Cars` table.

## Proposed Changes

### Data Layer
#### [MODIFY] [Car.cs](file:///d:/projects/CarNest/DAL/Entity/Car.cs)
- Add `public CarStatus Status { get; set; }` property.
- Default value should be `Pending` (or handled in logic).

#### [NEW] [CarStatus.cs](file:///d:/projects/CarNest/DAL/Enums/CarStatus.cs)
- Enum with values: `Pending`, `Approved`, `Rejected`.

### Business Logic
#### [MODIFY] [CarManager.cs](file:///d:/projects/CarNest/BLL/Manager/CarManager/CarManager.cs)
- `AddAsync`:
    - If `UserRole` is `Vendor`, set `Status = Pending`.
    - If `UserRole` is `Admin`, set `Status = Approved`.
- `GetAllAsync`:
    - Filter query to only include `Status == Approved`.
- `GetPendingCarsAsync` (New Method):
    - Returns cars where `Status == Pending`.
- `UpdateStatusAsync` (New Method):
    - Allows Admin to set status to `Approved` or `Rejected`.

### API Layer
#### [MODIFY] [CarController.cs](file:///d:/projects/CarNest/Presentation/Controllers/CarController.cs)
- `Add`: Logic is handled in Manager, but verify roles are passed correctly.
- `GetPending` (New Endpoint):
    - `[HttpGet("pending")]`
    - `[Authorize(Roles = "Admin")]`
- `UpdateStatus` (New Endpoint):
    - `[HttpPut("{id}/status")]`
    - `[Authorize(Roles = "Admin")]`

## Verification Plan

### Manual Verification
1.  **Vendor Post**: Log in as Vendor, post a car. Verify it does not appear in the main list.
2.  **Admin Check**: Log in as Admin. Verify the new car appears in the "Pending" list.
3.  **Approval**: Admin approves the car.
4.  **Public Check**: Verify the car now appears in the main list.
5.  **Rejection**: Create another car, reject it, ensure it stays hidden.
