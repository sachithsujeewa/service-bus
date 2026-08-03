namespace SourceApp;

internal static class DemoUi
{
    internal static string Page => """
        <!DOCTYPE html>
        <html lang="en"><head>
        <meta charset="utf-8"/><meta name="viewport" content="width=device-width, initial-scale=1"/>
        <title>Mock RamBase — Source App</title>
        <style>
        :root { --bg:#0f1419; --card:#1a2332; --border:#2d3a4f; --text:#e6edf3; --muted:#8b9cb3; --accent:#22c55e; }
        body { font-family: system-ui,sans-serif; background:var(--bg); color:var(--text); margin:0; padding:1.5rem; max-width:520px; }
        h1 { font-size:1.25rem; margin:0 0 0.25rem; }
        .sub { color:var(--muted); font-size:0.85rem; margin-bottom:1rem; }
        .badge { display:inline-block; background:#14532d; color:#86efac; font-size:0.7rem; padding:0.15rem 0.5rem;
                 border-radius:4px; margin-bottom:1rem; }
        .card { background:var(--card); border:1px solid var(--border); border-radius:8px; padding:1rem; }
        label { display:block; font-size:0.75rem; color:var(--muted); margin-bottom:0.25rem; }
        input, select { width:100%; padding:0.45rem; border:1px solid var(--border); border-radius:6px;
                        background:var(--bg); color:var(--text); margin-bottom:0.6rem; }
        button { background:var(--accent); color:#052e16; border:none; padding:0.6rem 1.2rem; border-radius:6px;
                 font-weight:600; cursor:pointer; width:100%; }
        pre { background:var(--bg); border:1px solid var(--border); border-radius:6px; padding:0.75rem;
              font-size:0.75rem; margin-top:1rem; white-space:pre-wrap; }
        pre.ok { border-color:#22c55e; color:#86efac; }
        pre.err { border-color:#ef4444; color:#fca5a5; }
        a { color:#3b82f6; font-size:0.85rem; }
        .quick { margin-top:0.75rem; font-size:0.85rem; }
        </style></head><body>
        <h1>Mock RamBase application</h1>
        <p class="sub">Step 1 — simulate a business event leaving RamBase</p>
        <span class="badge">source-app · :5101</span>

        <div class="card">
          <label>Event type</label>
          <select id="eventType"><option>OrderCreated</option><option>HldCreated</option><option>CustomerUpdated</option></select>
          <label>System ID</label>
          <input id="systemId" value="DEMO"/>
          <label>Database</label>
          <input id="database" value="ALL"/>
          <label>Order ID (parameter)</label>
          <input id="orderId" placeholder="auto-generated if empty"/>
          <button onclick="emit()">Emit event → Service Bus API</button>
          <p class="quick">Quick link (no form): <a href="/emit/OrderCreated" target="_blank">GET /emit/OrderCreated</a></p>
          <pre id="out">Ready.</pre>
        </div>

        <p><a href="http://localhost:8080/">Service Bus control panel</a> ·
           <a href="http://localhost:5102/">Partner inbox</a></p>

        <script>
        async function emit() {
          const eventType = document.getElementById('eventType').value;
          const orderId = document.getElementById('orderId').value.trim()
            || Math.random().toString(36).slice(2, 10);
          const body = {
            systemId: document.getElementById('systemId').value.trim() || 'DEMO',
            database: document.getElementById('database').value.trim() || 'ALL',
            parameters: { orderId }
          };
          const out = document.getElementById('out');
          out.textContent = 'Sending…';
          out.className = '';
          try {
            const r = await fetch('/emit/' + encodeURIComponent(eventType), {
              method: 'POST',
              headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
              body: JSON.stringify(body)
            });
            const text = await r.text();
            const ok = r.ok;
            out.textContent = (ok ? 'SUCCESS (' + r.status + ')\n' : 'FAILED (' + r.status + ')\n') + text;
            out.className = ok ? 'ok' : 'err';
          } catch (e) {
            out.textContent = 'FAILED\n' + e.message;
            out.className = 'err';
          }
        }
        </script>
        </body></html>
        """;
}
