# Sitecore Cleanup Monitor
The module consists of 3 agents that will monitor each the Event Queue, Publish Queue and History tables to ensure that they don't exceed a set threshold. 

### Why would you use it?
I many cases, Sitecore's default cleanup agents just aren't effecient enough in cleaning up these key Sitecore tables.

This module allows you to be proactive instead of reactive, so that you don't have to log into your SQL instance to manually run queries to clean up your tables, usually after the $#*!,$h* has hit the fan.

### How does it work?
When due, the agent will check the row count of the target table in each database (core, master and web), and if the count is above the set threshold, it will remove the oldest rows, bringing the row count down to that threshold. It won't do anything to tables below the threshold.

You can set how often you want each agent to run, and what you want your threshold / table row count to be. You also don't need to use all three agents. If you only want to monitor the Event Queue for example, simply comment or remove the other agents from the module's config file.

## Configuration
Defaults are set to run every 30 minutes and table threshold size is 900, making sure your instance is below the target 1000 rows (per Sitecore's Performance Tuning Guide)

```
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <scheduling>
      <agent type="Sitecore.Cleanup.PublishQueue, Sitecore.Cleanup" method="Run" interval="00:30:00">
        <Threshold>900</Threshold>
      </agent>
      <agent type="Sitecore.Cleanup.History, Sitecore.Cleanup" method="Run" interval="00:30:00">
        <Threshold>900</Threshold>
      </agent>
      <agent type="Sitecore.Cleanup.EventQueue, Sitecore.Cleanup" method="Run" interval="00:30:00">
        <Threshold>900</Threshold>
      </agent>
    </scheduling>
  </sitecore>
</configuration>

```

## Installation

The Sitecore package located in the Package folder called Sitecore Cleanup Monitor-1.0.zip contains:

* Binary (release build).
* Configuration file containing the 3 agents with default settings.

Use the Sitecore Installation Wizard to install the package. 

**After installation**:

* Configure the clean up agents run frequency and table threshold.
