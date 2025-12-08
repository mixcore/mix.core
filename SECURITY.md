# Security Policy

## Supported Versions

The following versions of Mixcore CMS are currently supported with security updates:

| Version | .NET Version | Supported          | End of Support |
| ------- | ------------ | ------------------ | -------------- |
| 2.x     | .NET 9.0     | :white_check_mark: | Current        |
| 1.x     | .NET 8.0     | :white_check_mark: | TBD            |
| < 1.0   | .NET 7.0     | :x:                | Ended          |

We recommend always using the latest stable version to ensure you have the most recent security patches.

## Reporting a Vulnerability

We take security vulnerabilities seriously. If you discover a security issue, please report it responsibly.

### How to Report

**DO NOT** open a public GitHub issue for security vulnerabilities.

Instead, please report security vulnerabilities by emailing:

**security@mixcore.org**

Or use GitHub's private vulnerability reporting feature:
1. Go to the [Security tab](https://github.com/mixcore/mix.core/security)
2. Click "Report a vulnerability"
3. Fill out the form with details about the vulnerability

### What to Include

Please include the following information in your report:

- **Description**: A clear description of the vulnerability
- **Steps to Reproduce**: Detailed steps to reproduce the issue
- **Impact**: The potential impact of the vulnerability
- **Affected Versions**: Which versions are affected
- **Proof of Concept**: If applicable, include code or screenshots
- **Suggested Fix**: If you have ideas on how to fix the issue

### What to Expect

| Timeframe | Action |
| --------- | ------ |
| 24-48 hours | Initial acknowledgment of your report |
| 1 week | Preliminary assessment and severity rating |
| 2-4 weeks | Fix development and testing |
| After fix | Public disclosure with credit (if desired) |

### Severity Ratings

We use the following severity ratings based on CVSS scores:

| Severity | CVSS Score | Response Time |
| -------- | ---------- | ------------- |
| Critical | 9.0 - 10.0 | Immediate |
| High     | 7.0 - 8.9  | 1-2 weeks |
| Medium   | 4.0 - 6.9  | 2-4 weeks |
| Low      | 0.1 - 3.9  | Next release |

### Recognition

We appreciate the security research community's efforts in helping keep Mixcore CMS secure. With your permission, we will:

- Acknowledge your contribution in the security advisory
- Add your name to our Security Hall of Fame
- Provide a letter of acknowledgment for your contribution

## Security Best Practices

When deploying Mixcore CMS, please follow these security best practices:

### Configuration

- [ ] Use strong, unique passwords for all accounts
- [ ] Enable HTTPS/TLS for all connections
- [ ] Configure proper CORS policies
- [ ] Set secure cookie attributes
- [ ] Disable debug mode in production
- [ ] Keep all dependencies up to date

### Database

- [ ] Use strong database credentials
- [ ] Restrict database access to application servers only
- [ ] Enable encryption at rest
- [ ] Regularly backup your database
- [ ] Use parameterized queries (handled by EF Core)

### Authentication

- [ ] Enable multi-factor authentication (MFA) for admin accounts
- [ ] Use secure password hashing (default: ASP.NET Core Identity)
- [ ] Implement account lockout policies
- [ ] Use JWT with appropriate expiration times
- [ ] Rotate secrets and keys regularly

### Infrastructure

- [ ] Use a Web Application Firewall (WAF)
- [ ] Enable rate limiting
- [ ] Configure proper logging and monitoring
- [ ] Perform regular security audits
- [ ] Keep the host operating system updated

## Security Features

Mixcore CMS includes the following built-in security features:

- **Authentication**: JWT-based authentication with OAuth 2.0 / OpenID Connect support
- **Authorization**: Role-based and policy-based access control
- **Input Validation**: Automatic model validation and sanitization
- **CSRF Protection**: Anti-forgery tokens for form submissions
- **XSS Prevention**: Output encoding and Content Security Policy headers
- **SQL Injection Prevention**: Parameterized queries via Entity Framework Core
- **Rate Limiting**: Configurable API rate limiting
- **Audit Logging**: Comprehensive logging of security-relevant events
- **Encryption**: Data encryption at rest and in transit

## Security Updates

Security updates are released as:

1. **Patch releases**: For critical and high severity issues
2. **Minor releases**: For medium and low severity issues
3. **Security advisories**: Published on GitHub Security Advisories

Subscribe to our security mailing list or watch this repository to receive notifications about security updates.

## Contact

For security-related questions that are not vulnerability reports, please contact:

- Email: security@mixcore.org
- GitHub Discussions: [Security category](https://github.com/mixcore/mix.core/discussions/categories/security)

Thank you for helping keep Mixcore CMS and its users safe!
