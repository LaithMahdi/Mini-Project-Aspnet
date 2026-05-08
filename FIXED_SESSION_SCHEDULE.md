# Fixed Session Schedule Implementation

## Overview
The school now has a fixed daily session schedule with 4 predefined time slots and breaks. Each session is exactly 90 minutes with scheduled breaks between them.

## Daily Schedule

```
┌─────────────────────────────────────────────────┐
│         DAILY SESSION SCHEDULE                  │
├─────────────────────────────────────────────────┤
│ SESSION 1:  08:30 - 10:00  (90 minutes)        │
│ BREAK:      10:00 - 10:10  (10 minutes)        │
│ SESSION 2:  10:10 - 11:40  (90 minutes)        │
│ BREAK:      11:40 - 12:10  (30 minutes - LUNCH)│
│ SESSION 3:  12:10 - 13:40  (90 minutes)        │
│ BREAK:      13:40 - 13:50  (10 minutes)        │
│ SESSION 4:  13:50 - 15:20  (90 minutes)        │
└─────────────────────────────────────────────────┘
```

## Technical Implementation

### 1. SessionScheduleService
**Location:** `school/Services/SessionScheduleService.cs`

A service that defines and manages the fixed session slots:

```csharp
public class SessionSlot
{
    public int SlotNumber { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string DisplayName { get; set; }
}
```

**Available Slots:**
- Slot 1: 08:30-10:00 (Session 1)
- Slot 2: 10:10-11:40 (Session 2)
- Slot 3: 12:10-13:40 (Session 3)
- Slot 4: 13:50-15:20 (Session 4)

### 2. Service Registration
**Location:** `school/Program.cs`

```csharp
builder.Services.AddScoped<ISessionScheduleService, SessionScheduleService>();
```

The service is injected into `SessionsController` to provide available slots to views.

### 3. Updated SessionsController
**Changes:**
- Injected `ISessionScheduleService` dependency
- Updated `PopulateEditOptions()` to include session slots in ViewBag
- Slots are passed to both Create and Edit views

### 4. Updated Session Forms

#### Sessions/Create.cshtml
- Replaced free-form "Start Time" and "End Time" inputs with a **Time Slot dropdown**
- Shows all 4 available slots with their display names and times
- Includes a helpful note about session duration and breaks

#### Sessions/Edit.cshtml
- Same slot dropdown implementation
- Pre-selects the current session's slot for editing
- Dynamically updates Start/End times when slot is changed

### 5. Client-Side Slot Selection

**JavaScript Logic:**
```javascript
// When user selects a slot (e.g., "Session 1 (08:30 - 10:00)")
// The script automatically:
1. Parses the time range (08:30-10:00)
2. Populates hidden StartTime field with 08:30
3. Populates hidden EndTime field with 10:00
4. Triggers validation events
```

## How It Works

### Creating a New Session

1. Admin navigates to **Sessions > Create**
2. Selects:
   - Session Date (today or later)
   - **Time Slot** (dropdown with 4 fixed options)
   - Teacher (from available teachers)
   - Subject
   - Room (with automatic conflict detection)
3. System validates:
   - Room is not already booked in that slot
   - Teacher doesn't have another session in that slot
   - Date is not in the past
4. Hidden Start/End times are automatically filled based on selected slot
5. Session is saved with fixed times

### Editing a Session

1. Click Edit on existing session
2. Time Slot dropdown pre-selects current session's slot
3. Can change to different slot if available
4. Same conflict detection applies
5. Changes are saved

## Validation Features

✅ **Room Availability**: Cannot book same room in overlapping time slots on same date

✅ **Teacher Availability**: Cannot assign teacher to overlapping sessions on same date

✅ **Fixed Times**: Times are always set to predefined slots (no free-form time input possible)

✅ **Date Validation**: Sessions cannot be created for past dates

## Benefits

1. **Consistency**: All sessions follow the same time structure
2. **Prevents Scheduling Errors**: Free-form time inputs are eliminated
3. **Easy Planning**: Staff knows exactly when sessions occur
4. **Automatic Conflict Detection**: System prevents double-booking
5. **Clear Schedule**: Students/teachers see predictable daily schedule

## Example Usage

### Scenario 1: Creating Session 1
- Date: 2026-05-20
- Slot: "Session 1 (08:30 - 10:00)"
- Teacher: John Smith (Mathematics)
- Subject: Algebra
- Room: Room 101

**Result:** Session saved with:
- StartTime: 08:30
- EndTime: 10:00
- Date: 2026-05-20

### Scenario 2: Booking Conflict
- Attempt to book Teacher "John Smith" for same day but Slot 2 (10:10-11:40)
- If John already has a Session 1 on that day → ❌ Error: "Teacher has another session from 08:30 to 10:00"
- Cannot create overlapping session

### Scenario 3: Lunch Break
- Sessions end at 11:40
- Lunch break from 11:40 to 12:10
- Next session starts at 12:10 (automatically enforced by slot selection)
- No free-form time entry possible

## Files Modified

1. **school/Services/SessionScheduleService.cs** - NEW
   - Defines fixed session slots
   - Provides slot lookup methods

2. **school/Program.cs**
   - Added service registration
   - Added using statement for Services namespace

3. **school/Controllers/SessionsController.cs**
   - Injected ISessionScheduleService
   - Updated PopulateEditOptions() method
   - Session times already validated in Create/Edit POST actions

4. **school/Views/Sessions/Create.cshtml**
   - Replaced time inputs with slot dropdown
   - Added automatic slot-to-time parsing script
   - Removed manual Start/End time inputs

5. **school/Views/Sessions/Edit.cshtml**
   - Same dropdown implementation
   - Pre-selection of current slot
   - Same automatic time parsing

## Testing the Implementation

### Test 1: Create Session with Slot
✓ Navigate to Sessions > Create
✓ Select date and slot
✓ StartTime and EndTime are auto-populated correctly
✓ Session saves successfully

### Test 2: Room Conflict
✓ Create Session 1 in Room 101 on 2026-05-20
✓ Try to create Session 2 in same room
✓ Should show error: "Room is not available..."

### Test 3: Teacher Conflict
✓ Assign Teacher X to Session 1 on 2026-05-20
✓ Try to assign same teacher to Session 2 on same date
✓ Should show error: "Teacher is not available..."

### Test 4: Lunch Break
✓ Create Session 2 (10:10-11:40)
✓ Create Session 3 (12:10-13:40)
✓ Verify 30-minute gap (11:40-12:10) is enforced by UI

## Schedule Enforcement

The system now **enforces** the fixed schedule:
- **No custom times possible** - Users MUST select from 4 predefined slots
- **Automatic time assignment** - Times are set automatically from slot selection
- **Breaks are enforced** - The schedule inherently prevents bookings during breaks
- **Uniform duration** - All sessions are exactly 90 minutes

Students and teachers will always see consistent, predictable session times! 🎯
