namespace ServiceBus.Api;

internal static class DemoUi
{
    internal const string SharedStyles = """
        :root { --bg:#0f1419; --card:#1a2332; --border:#2d3a4f; --text:#e6edf3; --muted:#8b9cb3;
                --accent:#3b82f6; --ok:#22c55e; --warn:#f59e0b; }
        * { box-sizing: border-box; }
        body { font-family: system-ui, sans-serif; background: var(--bg); color: var(--text);
               margin: 0; padding: 1.5rem; line-height: 1.5; }
        h1 { font-size: 1.35rem; margin: 0 0 0.25rem; }
        h2 { font-size: 1rem; margin: 0 0 0.75rem; color: var(--muted); font-weight: 500; }
        .sub { color: var(--muted); font-size: 0.85rem; margin-bottom: 1.25rem; }
        .grid { display: grid; gap: 1rem; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); }
        .card { background: var(--card); border: 1px solid var(--border); border-radius: 8px; padding: 1rem; }
        label { display: block; font-size: 0.75rem; color: var(--muted); margin-bottom: 0.25rem; }
        input, select, textarea { width: 100%; padding: 0.45rem 0.6rem; border: 1px solid var(--border);
            border-radius: 6px; background: var(--bg); color: var(--text); font-size: 0.9rem; }
        button { background: var(--accent); color: #fff; border: none; padding: 0.5rem 1rem;
            border-radius: 6px; cursor: pointer; font-size: 0.9rem; margin-top: 0.5rem; }
        button:hover { filter: brightness(1.1); }
        a { color: var(--accent); }
        .links { display: flex; flex-wrap: wrap; gap: 0.75rem; margin-bottom: 1rem; }
        .links a { background: var(--card); border: 1px solid var(--border); padding: 0.4rem 0.75rem;
                   border-radius: 6px; text-decoration: none; font-size: 0.85rem; }
        .flow { display: flex; align-items: center; gap: 0.5rem; flex-wrap: wrap; font-size: 0.8rem;
                color: var(--muted); margin-bottom: 1rem; }
        .flow span { background: var(--card); border: 1px solid var(--border); padding: 0.25rem 0.5rem;
                     border-radius: 4px; }
        .flow .arrow { color: var(--accent); }
        pre { background: var(--bg); border: 1px solid var(--border); border-radius: 6px;
              padding: 0.75rem; font-size: 0.75rem; overflow: auto; max-height: 200px; white-space: pre-wrap; }
        .ok { color: var(--ok); }
        .err { color: #ef4444; }
        .row { margin-bottom: 0.6rem; }
        """;

    internal static string HubPage => """
        <!DOCTYPE html>
        <html lang="en"><head>
        <meta charset="utf-8"/><meta name="viewport" content="width=device-width, initial-scale=1"/>
        <title>Service Bus MVP — Control Panel</title>
        <style>
        """ + SharedStyles + """
        </style>
        </head><body>
        <h1>Service Bus MVP</h1>
        <h2>Control panel · port 8080</h2>
        <p class="sub">Registry, publish API, DLQ. Use the demo apps for the full story.</p>

        <div class="flow">
          <span>1 Source</span><span class="arrow">→</span>
          <span>2 API</span><span class="arrow">→</span>
          <span>3 RabbitMQ</span><span class="arrow">→</span>
          <span>4 Dispatcher</span><span class="arrow">→</span>
          <span>5 Partner</span>
        </div>

        <div class="links">
          <a href="http://localhost:5101/" target="_blank">Source app (5101)</a>
          <a href="http://localhost:5102/" target="_blank">Partner app (5102)</a>
          <a href="http://localhost:15672/" target="_blank">RabbitMQ UI (15672)</a>
        </div>

        <div class="grid">
          <div class="card">
            <h2>Publish event</h2>
            <div class="row"><label>API key (source)</label>
              <input id="sourceKey" value="source-dev-key"/></div>
            <div class="row"><label>System ID</label><input id="systemId" value="DEMO"/></div>
            <div class="row"><label>Event type</label>
              <select id="eventType"><option>OrderCreated</option><option>HldCreated</option><option>CustomerUpdated</option></select></div>
            <div class="row"><label>Database</label><input id="database" value="ALL"/></div>
            <div class="row"><label>Parameter orderId</label><input id="orderId" placeholder="auto"/></div>
            <button onclick="publishEvent()">Publish to broker</button>
            <pre id="publishOut">—</pre>
          </div>

          <div class="card">
            <h2>Register webhook</h2>
            <div class="row"><label>API key (partner)</label>
              <input id="partnerKey" value="partner-dev-key"/></div>
            <div class="row"><label>Target URL</label>
              <input id="targetUrl" value="http://partner-app:5102/webhook"/></div>
            <div class="row"><label>Event type</label><input id="regEventType" value="OrderCreated"/></div>
            <div class="row"><label>HMAC secret</label><input id="hmacSecret" value="demo-hmac-secret"/></div>
            <button onclick="registerWebhook()">Register</button>
            <pre id="registerOut">—</pre>
          </div>

          <div class="card">
            <h2>Webhooks</h2>
            <button onclick="loadWebhooks()">Refresh list</button>
            <pre id="webhooksOut">Click Refresh</pre>
          </div>

          <div class="card">
            <h2>Dead letter queue</h2>
            <div class="row"><label>API key (admin)</label>
              <input id="adminKey" value="admin-dev-key"/></div>
            <button onclick="loadDlq()">Load DLQ</button>
            <pre id="dlqOut">—</pre>
          </div>
        </div>

        <script>
        async function apiCall(path, key, method, body) {
          method = method || 'GET';
          const opts = { method, headers: { 'X-Api-Key': key, 'Content-Type': 'application/json' } };
          if (body) opts.body = JSON.stringify(body);
          const r = await fetch(path, opts);
          const text = await r.text();
          return { status: r.status, text };
        }
        async function publishEvent() {
          const orderId = document.getElementById('orderId').value || Math.random().toString(36).slice(2, 10);
          const body = {
            systemId: document.getElementById('systemId').value,
            eventType: document.getElementById('eventType').value,
            database: document.getElementById('database').value,
            parameters: { orderId }
          };
          const out = document.getElementById('publishOut');
          try {
            const r = await apiCall('/api/events', document.getElementById('sourceKey').value, 'POST', body);
            out.textContent = r.status + '\n' + r.text;
            out.className = r.status < 300 ? 'ok' : 'err';
          } catch (e) { out.textContent = e.message; out.className = 'err'; }
        }
        async function registerWebhook() {
          const body = {
            targetUrl: document.getElementById('targetUrl').value,
            eventType: document.getElementById('regEventType').value,
            database: 'ALL',
            hmacSecret: document.getElementById('hmacSecret').value
          };
          const out = document.getElementById('registerOut');
          const r = await apiCall('/api/webhooks', document.getElementById('partnerKey').value, 'POST', body);
          out.textContent = r.status + '\n' + r.text;
        }
        async function loadWebhooks() {
          const key = document.getElementById('partnerKey').value;
          const r = await apiCall('/api/webhooks', key);
          document.getElementById('webhooksOut').textContent = r.status + '\n' + r.text;
        }
        async function loadDlq() {
          const r = await apiCall('/api/dlq', document.getElementById('adminKey').value);
          document.getElementById('dlqOut').textContent = r.status + '\n' + r.text;
        }
        loadWebhooks();
        </script>
        </body></html>
        """;
}
