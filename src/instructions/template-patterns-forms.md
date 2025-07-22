# Form Templates (folderType: 3)

Form templates handle user input, data collection, and form processing. They provide interactive elements for user engagement and data submission.

---

## Overview

Form templates (`folderType: 3`) are specialized for creating interactive forms, surveys, contact forms, and data collection interfaces.

### Key Characteristics
- **Purpose:** User input and data collection
- **Model:** `@model dynamic` or custom form models
- **Usage:** Embedded in pages or standalone forms
- **Features:** Validation, submission handling, data processing

---

## Creating Form Templates

### MCP Command
```csharp
CreateTemplate(
    folderType: 3,
    fileName: "ContactForm",
    extension: ".cshtml",
    mixThemeId: 1,
    content: "<form class=\"contact-form\"><!-- form content --></form>"
)
```

### Basic Form Template Structure
```razor
@model dynamic

<div class="form-container">
    <form class="custom-form" method="post" action="/submit-form">
        <div class="form-group">
            <label for="name">Name *</label>
            <input type="text" id="name" name="name" class="form-control" required>
        </div>
        
        <div class="form-group">
            <label for="email">Email *</label>
            <input type="email" id="email" name="email" class="form-control" required>
        </div>
        
        <div class="form-group">
            <label for="message">Message *</label>
            <textarea id="message" name="message" class="form-control" rows="5" required></textarea>
        </div>
        
        <button type="submit" class="btn btn-primary">Submit</button>
    </form>
</div>
```

---

## Form Template Examples

### Contact Form
```razor
@model dynamic

<div class="contact-form-container">
    <div class="form-header">
        <h3>Get in Touch</h3>
        <p>We'd love to hear from you. Send us a message and we'll respond as soon as possible.</p>
    </div>
    
    <form class="contact-form" id="contactForm">
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="firstName">First Name *</label>
                    <input type="text" id="firstName" name="firstName" class="form-control" required>
                    <div class="invalid-feedback"></div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="lastName">Last Name *</label>
                    <input type="text" id="lastName" name="lastName" class="form-control" required>
                    <div class="invalid-feedback"></div>
                </div>
            </div>
        </div>
        
        <div class="form-group">
            <label for="email">Email Address *</label>
            <input type="email" id="email" name="email" class="form-control" required>
            <div class="invalid-feedback"></div>
        </div>
        
        <div class="form-group">
            <label for="phone">Phone Number</label>
            <input type="tel" id="phone" name="phone" class="form-control">
        </div>
        
        <div class="form-group">
            <label for="subject">Subject *</label>
            <select id="subject" name="subject" class="form-control" required>
                <option value="">Please select a subject</option>
                <option value="general">General Inquiry</option>
                <option value="support">Technical Support</option>
                <option value="sales">Sales Question</option>
                <option value="partnership">Partnership Opportunity</option>
            </select>
            <div class="invalid-feedback"></div>
        </div>
        
        <div class="form-group">
            <label for="message">Message *</label>
            <textarea id="message" name="message" class="form-control" rows="6" 
                      placeholder="Please provide details about your inquiry..." required></textarea>
            <div class="invalid-feedback"></div>
        </div>
        
        <div class="form-group">
            <div class="form-check">
                <input type="checkbox" id="newsletter" name="newsletter" class="form-check-input">
                <label for="newsletter" class="form-check-label">
                    Subscribe to our newsletter for updates and special offers
                </label>
            </div>
        </div>
        
        <div class="form-actions">
            <button type="submit" class="btn btn-primary btn-lg">
                <span class="submit-text">Send Message</span>
                <span class="loading-text d-none">
                    <i class="fas fa-spinner fa-spin"></i> Sending...
                </span>
            </button>
        </div>
    </form>
    
    <div class="form-success d-none">
        <div class="alert alert-success">
            <i class="fas fa-check-circle"></i>
            <strong>Thank you!</strong> Your message has been sent successfully. We'll get back to you soon.
        </div>
    </div>
</div>

<style>
    .contact-form-container { max-width: 600px; margin: 0 auto; }
    .form-header { text-align: center; margin-bottom: 2rem; }
    .form-group { margin-bottom: 1.5rem; }
    .form-control { 
        border: 2px solid #e9ecef; 
        border-radius: 8px; 
        padding: 12px 15px;
        transition: border-color 0.3s ease;
    }
    .form-control:focus { 
        border-color: #007bff; 
        box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
    }
    .form-actions { text-align: center; margin-top: 2rem; }
    .btn-primary { 
        padding: 12px 40px; 
        border-radius: 25px; 
        font-weight: 600;
    }
    .invalid-feedback { display: block; color: #dc3545; font-size: 0.875rem; margin-top: 0.25rem; }
</style>

<script>
document.getElementById('contactForm').addEventListener('submit', function(e) {
    e.preventDefault();
    
    // Show loading state
    const submitBtn = this.querySelector('button[type="submit"]');
    const submitText = submitBtn.querySelector('.submit-text');
    const loadingText = submitBtn.querySelector('.loading-text');
    
    submitText.classList.add('d-none');
    loadingText.classList.remove('d-none');
    submitBtn.disabled = true;
    
    // Simulate form submission
    setTimeout(() => {
        this.style.display = 'none';
        document.querySelector('.form-success').classList.remove('d-none');
    }, 2000);
});
</script>
```

### Survey Form
```razor
@model dynamic

<div class="survey-form-container">
    <div class="survey-header">
        <h2>Customer Satisfaction Survey</h2>
        <p>Help us improve our services by sharing your feedback.</p>
        <div class="progress-bar">
            <div class="progress-fill" style="width: 0%"></div>
        </div>
    </div>
    
    <form class="survey-form" id="surveyForm">
        <!-- Section 1: Basic Information -->
        <div class="form-section" data-section="1">
            <h4>Section 1: Basic Information</h4>
            
            <div class="form-group">
                <label for="customerType">What type of customer are you?</label>
                <select id="customerType" name="customerType" class="form-control" required>
                    <option value="">Please select</option>
                    <option value="new">New Customer</option>
                    <option value="existing">Existing Customer</option>
                    <option value="former">Former Customer</option>
                </select>
            </div>
            
            <div class="form-group">
                <label for="serviceUsed">Which service did you use?</label>
                <div class="checkbox-group">
                    <div class="form-check">
                        <input type="checkbox" id="web-design" name="serviceUsed" value="web-design" class="form-check-input">
                        <label for="web-design" class="form-check-label">Web Design</label>
                    </div>
                    <div class="form-check">
                        <input type="checkbox" id="development" name="serviceUsed" value="development" class="form-check-input">
                        <label for="development" class="form-check-label">Development</label>
                    </div>
                    <div class="form-check">
                        <input type="checkbox" id="consulting" name="serviceUsed" value="consulting" class="form-check-input">
                        <label for="consulting" class="form-check-label">Consulting</label>
                    </div>
                    <div class="form-check">
                        <input type="checkbox" id="support" name="serviceUsed" value="support" class="form-check-input">
                        <label for="support" class="form-check-label">Support</label>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Section 2: Satisfaction Rating -->
        <div class="form-section d-none" data-section="2">
            <h4>Section 2: Satisfaction Rating</h4>
            
            <div class="form-group">
                <label>Overall, how satisfied are you with our service?</label>
                <div class="rating-group">
                    <input type="radio" id="rating5" name="overallRating" value="5">
                    <label for="rating5" class="rating-label">
                        <span class="rating-stars">★★★★★</span>
                        <span class="rating-text">Very Satisfied</span>
                    </label>
                    
                    <input type="radio" id="rating4" name="overallRating" value="4">
                    <label for="rating4" class="rating-label">
                        <span class="rating-stars">★★★★☆</span>
                        <span class="rating-text">Satisfied</span>
                    </label>
                    
                    <input type="radio" id="rating3" name="overallRating" value="3">
                    <label for="rating3" class="rating-label">
                        <span class="rating-stars">★★★☆☆</span>
                        <span class="rating-text">Neutral</span>
                    </label>
                    
                    <input type="radio" id="rating2" name="overallRating" value="2">
                    <label for="rating2" class="rating-label">
                        <span class="rating-stars">★★☆☆☆</span>
                        <span class="rating-text">Dissatisfied</span>
                    </label>
                    
                    <input type="radio" id="rating1" name="overallRating" value="1">
                    <label for="rating1" class="rating-label">
                        <span class="rating-stars">★☆☆☆☆</span>
                        <span class="rating-text">Very Dissatisfied</span>
                    </label>
                </div>
            </div>
            
            <div class="form-group">
                <label for="recommendationScore">How likely are you to recommend us to others? (0-10)</label>
                <input type="range" id="recommendationScore" name="recommendationScore" 
                       class="form-range" min="0" max="10" value="5">
                <div class="range-labels">
                    <span>Not at all likely</span>
                    <span class="range-value">5</span>
                    <span>Extremely likely</span>
                </div>
            </div>
        </div>
        
        <!-- Section 3: Feedback -->
        <div class="form-section d-none" data-section="3">
            <h4>Section 3: Additional Feedback</h4>
            
            <div class="form-group">
                <label for="improvements">What could we improve?</label>
                <textarea id="improvements" name="improvements" class="form-control" rows="4"
                          placeholder="Please share your suggestions for improvement..."></textarea>
            </div>
            
            <div class="form-group">
                <label for="additionalComments">Any additional comments?</label>
                <textarea id="additionalComments" name="additionalComments" class="form-control" rows="3"
                          placeholder="Share any other thoughts or feedback..."></textarea>
            </div>
            
            <div class="form-group">
                <div class="form-check">
                    <input type="checkbox" id="followUp" name="followUp" class="form-check-input">
                    <label for="followUp" class="form-check-label">
                        I'm willing to participate in follow-up research
                    </label>
                </div>
            </div>
        </div>
        
        <!-- Navigation Buttons -->
        <div class="form-navigation">
            <button type="button" class="btn btn-secondary" id="prevBtn" style="display: none;">Previous</button>
            <button type="button" class="btn btn-primary" id="nextBtn">Next</button>
            <button type="submit" class="btn btn-success" id="submitBtn" style="display: none;">Submit Survey</button>
        </div>
    </form>
</div>

<style>
    .survey-form-container { max-width: 700px; margin: 0 auto; }
    .survey-header { text-align: center; margin-bottom: 2rem; }
    .progress-bar { 
        width: 100%; 
        height: 8px; 
        background: #e9ecef; 
        border-radius: 4px; 
        margin-top: 1rem;
    }
    .progress-fill { 
        height: 100%; 
        background: #007bff; 
        border-radius: 4px; 
        transition: width 0.3s ease;
    }
    .form-section { margin-bottom: 2rem; }
    .form-section h4 { margin-bottom: 1.5rem; color: #495057; }
    .checkbox-group { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 10px; }
    .rating-group { display: flex; flex-direction: column; gap: 10px; }
    .rating-label { 
        display: flex; 
        align-items: center; 
        gap: 10px; 
        padding: 10px; 
        border: 2px solid #e9ecef; 
        border-radius: 8px; 
        cursor: pointer;
        transition: all 0.3s ease;
    }
    .rating-label:hover { border-color: #007bff; }
    input[type="radio"]:checked + .rating-label { border-color: #007bff; background: #f8f9fa; }
    .rating-stars { font-size: 1.2rem; color: #ffc107; }
    .form-range { margin: 10px 0; }
    .range-labels { 
        display: flex; 
        justify-content: space-between; 
        font-size: 0.875rem; 
        color: #666;
    }
    .range-value { font-weight: bold; color: #007bff; }
    .form-navigation { 
        display: flex; 
        justify-content: space-between; 
        margin-top: 2rem; 
        padding-top: 2rem; 
        border-top: 1px solid #e9ecef;
    }
</style>

<script>
let currentSection = 1;
const totalSections = 3;

// Update progress bar
function updateProgress() {
    const progress = (currentSection / totalSections) * 100;
    document.querySelector('.progress-fill').style.width = progress + '%';
}

// Show section
function showSection(section) {
    document.querySelectorAll('.form-section').forEach(s => s.classList.add('d-none'));
    document.querySelector(`[data-section="${section}"]`).classList.remove('d-none');
    
    // Update navigation buttons
    document.getElementById('prevBtn').style.display = section > 1 ? 'block' : 'none';
    document.getElementById('nextBtn').style.display = section < totalSections ? 'block' : 'none';
    document.getElementById('submitBtn').style.display = section === totalSections ? 'block' : 'none';
    
    updateProgress();
}

// Navigation event listeners
document.getElementById('nextBtn').addEventListener('click', function() {
    if (currentSection < totalSections) {
        currentSection++;
        showSection(currentSection);
    }
});

document.getElementById('prevBtn').addEventListener('click', function() {
    if (currentSection > 1) {
        currentSection--;
        showSection(currentSection);
    }
});

// Range slider value update
document.getElementById('recommendationScore').addEventListener('input', function() {
    document.querySelector('.range-value').textContent = this.value;
});

// Form submission
document.getElementById('surveyForm').addEventListener('submit', function(e) {
    e.preventDefault();
    alert('Thank you for your feedback! Your survey has been submitted.');
});

// Initialize
showSection(1);
</script>
```

### Registration Form
```razor
@model dynamic

<div class="registration-form-container">
    <div class="form-header">
        <h2>Create Your Account</h2>
        <p>Join our community and get access to exclusive content and features.</p>
    </div>
    
    <form class="registration-form" id="registrationForm">
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="firstName">First Name *</label>
                    <input type="text" id="firstName" name="firstName" class="form-control" required>
                    <div class="form-feedback"></div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="lastName">Last Name *</label>
                    <input type="text" id="lastName" name="lastName" class="form-control" required>
                    <div class="form-feedback"></div>
                </div>
            </div>
        </div>
        
        <div class="form-group">
            <label for="email">Email Address *</label>
            <input type="email" id="email" name="email" class="form-control" required>
            <div class="form-feedback"></div>
        </div>
        
        <div class="form-group">
            <label for="username">Username *</label>
            <input type="text" id="username" name="username" class="form-control" required>
            <small class="form-text">Must be 3-20 characters, letters and numbers only</small>
            <div class="form-feedback"></div>
        </div>
        
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <label for="password">Password *</label>
                    <input type="password" id="password" name="password" class="form-control" required>
                    <div class="password-strength">
                        <div class="strength-meter"></div>
                        <small class="strength-text">Password strength: <span>Weak</span></small>
                    </div>
                    <div class="form-feedback"></div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <label for="confirmPassword">Confirm Password *</label>
                    <input type="password" id="confirmPassword" name="confirmPassword" class="form-control" required>
                    <div class="form-feedback"></div>
                </div>
            </div>
        </div>
        
        <div class="form-group">
            <label for="birthDate">Date of Birth</label>
            <input type="date" id="birthDate" name="birthDate" class="form-control">
        </div>
        
        <div class="form-group">
            <label for="interests">Interests</label>
            <div class="interests-grid">
                <div class="form-check">
                    <input type="checkbox" id="tech" name="interests" value="technology" class="form-check-input">
                    <label for="tech" class="form-check-label">Technology</label>
                </div>
                <div class="form-check">
                    <input type="checkbox" id="design" name="interests" value="design" class="form-check-input">
                    <label for="design" class="form-check-label">Design</label>
                </div>
                <div class="form-check">
                    <input type="checkbox" id="business" name="interests" value="business" class="form-check-input">
                    <label for="business" class="form-check-label">Business</label>
                </div>
                <div class="form-check">
                    <input type="checkbox" id="marketing" name="interests" value="marketing" class="form-check-input">
                    <label for="marketing" class="form-check-label">Marketing</label>
                </div>
            </div>
        </div>
        
        <div class="form-group">
            <div class="form-check">
                <input type="checkbox" id="terms" name="terms" class="form-check-input" required>
                <label for="terms" class="form-check-label">
                    I agree to the <a href="/terms" target="_blank">Terms of Service</a> and 
                    <a href="/privacy" target="_blank">Privacy Policy</a> *
                </label>
            </div>
        </div>
        
        <div class="form-group">
            <div class="form-check">
                <input type="checkbox" id="newsletter" name="newsletter" class="form-check-input">
                <label for="newsletter" class="form-check-label">
                    Subscribe to our newsletter for updates and special offers
                </label>
            </div>
        </div>
        
        <div class="form-actions">
            <button type="submit" class="btn btn-primary btn-lg btn-block">
                Create Account
            </button>
        </div>
    </form>
    
    <div class="form-footer">
        <p>Already have an account? <a href="/login">Sign in here</a></p>
    </div>
</div>

<style>
    .registration-form-container { max-width: 600px; margin: 0 auto; }
    .form-header { text-align: center; margin-bottom: 2rem; }
    .form-group { margin-bottom: 1.5rem; }
    .form-control { 
        border: 2px solid #e9ecef; 
        border-radius: 8px; 
        padding: 12px 15px;
    }
    .password-strength { margin-top: 0.5rem; }
    .strength-meter { 
        width: 100%; 
        height: 4px; 
        background: #e9ecef; 
        border-radius: 2px; 
        overflow: hidden;
    }
    .strength-meter::after { 
        content: ''; 
        display: block; 
        height: 100%; 
        background: #dc3545; 
        width: 25%; 
        transition: all 0.3s ease;
    }
    .interests-grid { 
        display: grid; 
        grid-template-columns: repeat(auto-fit, minmax(150px, 1fr)); 
        gap: 10px; 
    }
    .form-feedback { font-size: 0.875rem; margin-top: 0.25rem; }
    .form-footer { text-align: center; margin-top: 2rem; }
</style>

<script>
// Password strength checker
document.getElementById('password').addEventListener('input', function() {
    const password = this.value;
    let strength = 0;
    
    if (password.length >= 8) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;
    
    const meter = document.querySelector('.strength-meter');
    const text = document.querySelector('.strength-text span');
    
    const colors = ['#dc3545', '#fd7e14', '#ffc107', '#28a745', '#17a2b8'];
    const labels = ['Very Weak', 'Weak', 'Fair', 'Good', 'Strong'];
    
    meter.style.background = `linear-gradient(to right, ${colors[strength - 1] || colors[0]} ${strength * 20}%, #e9ecef ${strength * 20}%)`;
    text.textContent = labels[strength - 1] || labels[0];
});

// Form validation
document.getElementById('registrationForm').addEventListener('submit', function(e) {
    e.preventDefault();
    
    // Basic validation example
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
    
    if (password !== confirmPassword) {
        alert('Passwords do not match!');
        return;
    }
    
    alert('Registration successful! Welcome to our community.');
});
</script>
```

---

## Best Practices

### Form Design
- **Clear labels:** Use descriptive, concise labels
- **Logical grouping:** Group related fields together
- **Progressive disclosure:** Break long forms into sections
- **Mobile-friendly:** Ensure forms work well on mobile devices

### Validation
- **Client-side validation:** Provide immediate feedback
- **Server-side validation:** Always validate on the server
- **Error messages:** Be specific and helpful
- **Success feedback:** Confirm successful submissions

### Accessibility
- **Label association:** Proper label-input relationships
- **Keyboard navigation:** All elements accessible via keyboard
- **Screen reader support:** Use ARIA attributes when needed
- **Color contrast:** Ensure sufficient contrast for visibility

---

## Next Steps

After creating form templates:

1. **Implement Backend Processing** - Handle form submissions
2. **Add Validation Logic** - Client and server-side validation
3. **Test Thoroughly** - Cross-browser and device testing
4. **Monitor Analytics** - Track form completion rates

---

## Related Guides

- **[Widget Templates](./template-patterns-widgets.md)** - Small interactive components
- **[Page Templates](./template-patterns-pages.md)** - Embedding forms in pages
- **[Template Patterns Overview](./template-patterns-overview.md)** - All template types
