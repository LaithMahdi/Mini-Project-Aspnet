# Custom CSS System Documentation

## Overview
The `custom-styles.css` file contains a comprehensive set of reusable CSS classes designed for a professional, modern UI system. These classes follow a consistent design language based on a primary color scheme and can be used across all views.

## Color Scheme
```css
--primary-color: #0f2b5b (Dark Blue)
--primary-light: #1a4a9e (Bright Blue)
--primary-gradient: linear-gradient(135deg, #0f2b5b, #1a4a9e)
--text-dark: #0f2b5b
--text-muted: #6b7280 (Gray)
--text-secondary: #9ca3af (Light Gray)
--border-color: #e5e7eb
--bg-light: #f8faff (Very light blue)
--success-bg: #dcfce7 (Light Green)
--danger-bg: #fee2e2 (Light Red)
```

## Class Reference

### Page Structure
- `.page-wrapper` - Main page container with font family
- `.page-header` - Header with title and action buttons
- `.page-header-left` - Left section with title and description
- `.page-header-right` - Right section with action buttons
- `.nav-breadcrumb` - Breadcrumb navigation

### Buttons
- `.btn-custom` - Base button class with flexbox layout
- `.btn-primary-gradient` - Blue gradient button (for primary actions)
- `.btn-secondary` - Light blue button (for secondary actions)
- `.btn-danger` - Red button (for delete/dangerous actions)

### Cards
- `.card-modern` - Card container with border and shadow
- `.card-header` - Card header section with title

### Forms
- `.form-group-custom` - Form field container
- `.form-label-custom` - Form label with custom styling
- `.form-control-custom` - Text input with custom styling
- `.form-check-custom` - Checkbox container
- `.form-check-input-custom` - Checkbox input
- `.form-check-label-custom` - Checkbox label
- `.form-actions` - Button container for submit/cancel
- `.form-error` - Error message styling
- `.validation-summary` - Validation error summary box

### Badges & Indicators
- `.badge-code` - Code badge (blue background)
- `.badge-active` - Active/inactive status badge
- `.badge-active.yes` - Active badge (green)
- `.badge-active.no` - Inactive badge (red)

### Tables
- `.table-modern` - Modern table styling
- `.action-links` - Action button container in tables
- `.action-btn` - Action button in tables
- `.action-btn.edit` - Edit button
- `.action-btn.details` - Details button
- `.action-btn.delete` - Delete button

### Stats & Cards
- `.stats-strip` - Grid container for stat cards
- `.stat-card` - Single stat card
- `.stat-card.accent` - Highlighted stat card with gradient
- `.stat-label` - Stat label text
- `.stat-num` - Stat number

### Typography
- `.font-playfair` - Playfair Display serif font
- `.font-dmsans` - DM Sans sans-serif font

### Utility Classes
- `.text-muted` - Muted gray text
- `.text-secondary` - Secondary gray text
- `.text-danger` - Red danger text
- `.text-success` - Green success text
- `.mb-0`, `.mb-1`, `.mb-2`, `.mb-3` - Margin bottom utilities
- `.mt-2`, `.mt-3` - Margin top utilities
- `.pt-2`, `.pb-2` - Padding utilities

## Usage Examples

### Create Page
```html
<div class="page-wrapper">
    <div class="page-header">
        <div class="page-header-left">
            <h1>Create Section</h1>
            <p>Add a new academic section</p>
        </div>
    </div>

    <div class="card-modern" style="max-width: 600px;">
        <div class="card-header">
            <h2>Section Details</h2>
        </div>

        <form>
            <div class="form-group-custom">
                <label class="form-label-custom">Name</label>
                <input class="form-control-custom" type="text" />
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

### Index/List Page
```html
<div class="page-wrapper">
    <div class="page-header">
        <div class="page-header-left">
            <h1>Items</h1>
            <p>Manage all items</p>
        </div>
        <div class="page-header-right">
            <a href="#" class="btn-custom btn-primary-gradient">New Item</a>
        </div>
    </div>

    <div class="stats-strip">
        <div class="stat-card accent">
            <div class="stat-label">Total</div>
            <div class="stat-num">42</div>
        </div>
        <div class="stat-card">
            <div class="stat-label">Active</div>
            <div class="stat-num">35</div>
        </div>
    </div>

    <div class="card-modern" style="padding: 0;">
        <table class="table-modern">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Status</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Item 1</td>
                    <td>
                        <span class="badge-active yes">
                            <span class="dot"></span>Active
                        </span>
                    </td>
                    <td>
                        <div class="action-links">
                            <a href="#" class="action-btn edit">Edit</a>
                            <a href="#" class="action-btn delete">Delete</a>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
```

## Responsive Design
All classes are mobile-responsive:
- Buttons stack vertically on small screens
- Grid layouts automatically adjust
- Tables remain readable on mobile
- Forms maintain usability on all device sizes

## Customization
To customize colors, edit the CSS variables at the top of `custom-styles.css`:
```css
:root {
    --primary-color: #0f2b5b;
    --primary-light: #1a4a9e;
    /* ... other variables ... */
}
```

All components will automatically use the new colors throughout the application.
