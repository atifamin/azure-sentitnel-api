using System;
using System.Collections.Generic;
using System.Text;

namespace AzureSentinel_ManagementAPI
{
    class ProjectRequirements
    {
    }

    //2/ Add these functions: 
    //authenticate multiple tenant
    //Create and Update Multiple alert Rules for 1 tenant : list of alert rules, post it to 1 tenant
    //Create an alert rule for multiple tenants: have 1 alert rule, post it to multiple tenants
    //Get all alert rules for all tenants

    //examples for these 3 functions:
    //public async Task<string> CreateMultipleMicrosoftSecurityIncidentCreationAlertRules(List<AlertRulePayload> alertrules)
    //{
    //    Loop post each of alert rule
    //        Authenticate one
    //    }

    //public async Task<string> CreateMultipleAlertRulesforMultipleTenants(List<tenant> tenants, AlertRulePayload alertrule)
    //{
    //    1 alert rule
    //        Authenticate multiple tenants
    //    }

    //public async Task<string> GetAllALertRulesFromMultipleTenants(List<tenant> tenants)
    //{
    //    Authenticate list of tenants
    //        Get all alert rules for each of tenant
    //    }

    //1. Refactor the existing code

}
