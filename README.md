Sample project to take events from a hub (EventHub triggered Azure Function) - deserialise the JSON (assuming JSON formatted messages) and checks a field to see where the message should be forwarded.

Based on values the message will be forwarded to different event hubs. This could be useful for separating messages to different queues based on type where the original application cannot be immediately refactored.
