# MIXCORE LICENSE IMPLEMENTATION GUIDE

This guide provides practical instructions for implementing the new Mixcore licensing structure for both project maintainers and users.

## FOR MIXCORE MAINTAINERS

### 1. LICENSE TRANSITION PLAN

#### Phase 1: Preparation (1-2 months)
- Finalize all license documents
- Prepare website infrastructure for license sales
- Create license validation mechanisms
- Inform the community about upcoming changes

#### Phase 2: Soft Launch (1 month)
- Introduce the new license structure while maintaining compatibility with the previous license
- Begin offering commercial licenses
- Gather feedback and adjust as needed

#### Phase 3: Full Implementation (after soft launch)
- Apply the new license to all repositories
- Implement technical validation for commercial licenses
- Begin enforcing attribution requirements

### 2. WEBSITE UPDATES

#### License Page Requirements
- Clear explanation of the Source Available License
- Visual comparison of different license tiers
- Self-service purchasing system for commercial licenses
- FAQ section addressing common questions
- Revenue tier calculator

#### Technical Implementation
- License key generation system
- Customer portal for license management
- Integration with payment processors
- Automated renewal notifications

### 3. CODE REPOSITORY UPDATES

#### Required Files
- `LICENSE.md` - Full text of the Mixcore Source Available License
- `COMMERCIAL.md` - Information about commercial licensing options
- `CONTRIBUTING.md` - Updated to reflect the new license terms for contributors

#### Attribution Implementation
- Add the following code to ensure proper attribution display:

```html
<!-- Attribution Component -->
<div id="mixcore-attribution" style="position: fixed; bottom: 20px; right: 20px; z-index: 1000;">
  <a href="https://www.mixcore.org" target="_blank" rel="noopener">
    powered by Mixcore CMS
  </a>
</div>
```

#### License Validation Code
- Implement a license validation API endpoint
- Add client-side license validation
- Create a configuration option for commercial license keys

## FOR MIXCORE USERS

### 1. UNDERSTANDING YOUR LICENSE REQUIREMENTS

#### Step 1: Determine Your Organization Type
- Individual developer or small business (under $1M annual revenue)
- Medium-sized business ($1M-$10M annual revenue)
- Large organization (over $10M annual revenue)
- Digital agency (creating websites for clients)
- SaaS provider or product company

#### Step 2: Check Geographical Adjustments
- Identify your country's World Bank classification
- Apply the appropriate adjustment to revenue thresholds
- Verify if special regional pricing is available

#### Step 3: Evaluate Usage Requirements
- Internal use only
- Customer-facing applications
- Multiple deployments
- White-labeled solutions
- SaaS offering

### 2. IMPLEMENTING THE ATTRIBUTION REQUIREMENT

#### Default Attribution Code
```html
<div class="mixcore-attribution">
  <a href="https://www.mixcore.org" target="_blank" rel="noopener">
    powered by Mixcore CMS
  </a>
</div>
```

#### CSS Styling Guidelines
- Ensure text is clearly visible (minimum contrast ratio: 4.5:1)
- Minimum font size: 12px
- Position consistently across user interfaces
- Do not hide on scroll or with CSS tricks

#### Attribution Placement Options
- Footer (recommended)
- Dashboard sidebar
- About/Credits page (must still be visible on each interface)

### 3. COMMERCIAL LICENSE IMPLEMENTATION

#### Step 1: Purchase the Appropriate License
- Visit the Mixcore website
- Select the appropriate license tier
- Complete the purchase process
- Receive license key by email

#### Step 2: Implement License Key
- Add the license key to your Mixcore configuration
- Validate the license is working correctly
- Remove attribution if your license permits

```json
// Example configuration in appsettings.json
{
  "MixcoreLicense": {
    "LicenseKey": "YOUR-LICENSE-KEY-HERE",
    "LicenseTier": "Standard", // Automatically detected from key
    "DisableAttribution": true // Only effective with valid license
  }
}
```

#### Step 3: Verify Compliance
- Use the Mixcore License Validator tool
- Ensure attribution is removed if applicable
- Confirm license covers all deployments

### 4. COMPLIANCE CHECKLIST

#### Source Available License Compliance
- [ ] Attribution properly displayed on all user interface screens
- [ ] Attribution links to https://www.mixcore.org
- [ ] Not offering as SaaS if subject to Commons Clause
- [ ] Distributing source code with any modifications

#### Commercial License Compliance
- [ ] Valid license key implemented
- [ ] License covers all deployments
- [ ] Using features permitted by license tier
- [ ] License renewed before expiration

## LICENSE VALIDATION API

For technical teams implementing license validation, the Mixcore License API is available at:
`https://api.mixcore.org/license/validate`

### Request Format
```json
{
  "licenseKey": "YOUR-LICENSE-KEY",
  "domain": "yourdomain.com",
  "deploymentId": "unique-deployment-identifier"
}
```

### Response Format
```json
{
  "valid": true,
  "tier": "Standard",
  "expires": "2023-12-31T23:59:59Z",
  "permissions": {
    "disableAttribution": true,
    "commercialUse": true,
    "multipleDeployments": false,
    "saasOffering": false
  }
}
```

## ADDITIONAL RESOURCES

- License FAQ: https://www.mixcore.org/license/faq
- Commercial License Purchase: https://www.mixcore.org/license/purchase
- Technical Support: https://www.mixcore.org/support
- Legal Contact: legal@mixcore.org
- Sales Contact: sales@mixcore.org

---

This implementation guide is provided for informational purposes and may be updated. Always refer to the official license documents for definitive terms and conditions.