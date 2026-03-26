# Quick CSS Class Reference Guide

## Button Classes
```html
<!-- Primary Action (Blue Gradient) -->
<button class="btn-custom btn-primary-gradient">Save Changes</button>

<!-- Secondary Action (Light Blue) -->
<a href="#" class="btn-custom btn-secondary">Cancel</a>

<!-- Danger Action (Red) -->
<a href="#" class="btn-custom btn-danger">Delete</a>
```

## Form Elements
```html
<!-- Form Group -->
<div class="form-group-custom">
    <label class="form-label-custom">Label Text</label>
    <input class="form-control-custom" type="text" placeholder="Enter text" />
</div>

<!-- Checkbox -->
<div class="form-check-custom">
    <input type="checkbox" class="form-check-input-custom" id="check1" />
    <label for="check1" class="form-check-label-custom">
        Checkbox label
    </label>
</div>

<!-- Form Actions -->
<div class="form-actions">
    <button type="submit" class="btn-custom btn-primary-gradient">Submit</button>
    <a href="#" class="btn-custom btn-secondary">Cancel</a>
</div>
```

## Card Layouts
```html
<!-- Basic Card -->
<div class="card-modern">
    <div class="card-header">
        <h2>Card Title</h2>
    </div>
    <!-- Card content here -->
</div>

<!-- Card as Table Wrapper -->
<div class="card-modern" style="padding: 0; overflow: hidden;">
    <table class="table-modern">
        <!-- Table content -->
    </table>
</div>
```

## Typography
```html
<!-- Page Header -->
<div class="page-header">
    <div class="page-header-left">
        <h1>Page Title</h1>
        <p>Page subtitle or description</p>
    </div>
    <div class="page-header-right">
        <a href="#" class="btn-custom btn-primary-gradient">Action</a>
    </div>
</div>

<!-- Breadcrumb Navigation -->
<div class="nav-breadcrumb">
    <a href="#">Home</a>
    <span class="separator">/</span>
    <a href="#">Items</a>
    <span class="separator">/</span>
    <span>Create</span>
</div>
```

## Badges
```html
<!-- Status Badge - Active -->
<span class="badge-active yes">
    <span class="dot"></span>Active
</span>

<!-- Status Badge - Inactive -->
<span class="badge-active no">
    <span class="dot"></span>Inactive
</span>

<!-- Code Badge -->
<span class="badge-code">CODE-123</span>
```

## Tables
```html
<!-- Modern Table -->
<table class="table-modern">
    <thead>
        <tr>
            <th>Column 1</th>
            <th>Column 2</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
            <td>
                <div class="action-links">
                    <a href="#" class="action-btn edit">Edit</a>
                    <a href="#" class="action-btn details">Details</a>
                    <a href="#" class="action-btn delete">Delete</a>
                </div>
            </td>
        </tr>
    </tbody>
</table>
```

## Statistics
```html
<!-- Stats Grid -->
<div class="stats-strip">
    <!-- Highlighted Card -->
    <div class="stat-card accent">
        <div class="stat-label">Total Items</div>
        <div class="stat-num">125</div>
    </div>
    
    <!-- Regular Card -->
    <div class="stat-card">
        <div class="stat-label">Active</div>
        <div class="stat-num">98</div>
    </div>
</div>
```

## Error Messages
```html
<!-- Validation Summary -->
<div class="validation-summary">
    <ul>
        <li>Error message 1</li>
        <li>Error message 2</li>
    </ul>
</div>

<!-- Field Error -->
<span class="form-error">This field is required</span>
```

## Empty States
```html
<!-- Empty State Message -->
<div class="empty-state">
    <svg><!-- Your icon here --></svg>
    <p>No items found. <a href="#">Create one now.</a></p>
</div>
```

## Utility Classes

### Margin Bottom
- `.mb-0` - 0 margin
- `.mb-1` - 0.5rem margin
- `.mb-2` - 1rem margin
- `.mb-3` - 1.5rem margin

### Margin Top
- `.mt-2` - 1rem margin
- `.mt-3` - 1.5rem margin

### Padding
- `.pt-2` - 1rem padding top
- `.pb-2` - 1rem padding bottom

### Text Colors
- `.text-muted` - Muted gray
- `.text-secondary` - Secondary gray
- `.text-danger` - Red
- `.text-success` - Green

## Color Variables (for custom CSS)
```css
--primary-color: #0f2b5b
--primary-light: #1a4a9e
--primary-gradient: linear-gradient(135deg, #0f2b5b, #1a4a9e)
--text-dark: #0f2b5b
--text-muted: #6b7280
--text-secondary: #9ca3af
--border-color: #e5e7eb
--bg-light: #f8faff
--success-color: #166534
--success-bg: #dcfce7
--danger-color: #991b1b
--danger-bg: #fee2e2
--info-color: #3730a3
--info-bg: #eef2ff
```

## Complete Form Example
```html
<div class="page-wrapper">
    <div class="page-header">
        <div class="page-header-left">
            <h1>Edit Item</h1>
            <p>Update item information</p>
        </div>
    </div>

    <div class="card-modern" style="max-width: 600px;">
        <div class="card-header">
            <h2>Item Details</h2>
        </div>

        <form>
            <div asp-validation-summary="ModelOnly" class="validation-summary"></div>

            <div class="form-group-custom">
                <label class="form-label-custom">Item Name</label>
                <input class="form-control-custom" type="text" />
                <span class="form-error">Field is required</span>
            </div>

            <div class="form-check-custom">
                <input type="checkbox" class="form-check-input-custom" id="active" />
                <label for="active" class="form-check-label-custom">
                    Mark as active
                </label>
            </div>

            <div class="form-actions">
                <button type="submit" class="btn-custom btn-primary-gradient">
                    Save
                </button>
                <a href="#" class="btn-custom btn-secondary">Cancel</a>
            </div>
        </form>
    </div>
</div>
```

## Tips
1. **Always wrap forms in `.card-modern`** for consistent styling
2. **Use `.form-actions`** to group submit and cancel buttons
3. **Never override** the base button class `.btn-custom` without modifiers
4. **Use CSS variables** if you need to customize colors
5. **For tables**, wrap in `.card-modern` with `padding: 0; overflow: hidden;`
6. **Use `.action-links`** for multiple buttons in table rows
