namespace PartnerApp;

internal static class DemoUi
{
    internal static string Page => """
        <!DOCTYPE html>
        <html lang="en"><head>
        <meta charset="utf-8"/><meta name="viewport" content="width=device-width, initial-scale=1"/>
        <title>External Partner — Webhook Receiver</title>
        <style>
        :root { --bg:#0f1419; --card:#1a2332; --border:#2d3a4f; --text:#e6edf3; --muted:#8b9cb3; --accent:#a855f7; }
        body { font-family: system-ui,sans-serif; background:var(--bg); color:var(--text); margin:0; padding:1.5rem; }
        h1 { font-size:1.25rem; margin:0 0 0.25rem; }
        .sub { color:var(--muted); font-size:0.85rem; margin-bottom:1rem; }
        .badge { display:inline-block; background:#581c87; color:#d8b4fe; font-size:0.7rem; padding:0.15rem 0.5rem;
                 border-radius:4px; margin-bottom:1rem; }
        .card { background:var(--card); border:1px solid var(--border); border-radius:8px; padding:1rem; }
        table { width:100%; border-collapse:collapse; font-size:0.8rem; }
        th, td { border-bottom:1px solid var(--border); padding:0.5rem; text-align:left; vertical-align:top; }
        th { color:var(--muted); font-weight:500; }
        .count { font-size:1.5rem; font-weight:600; color:var(--accent); }
        button { background:var(--accent); color:#fff; border:none; padding:0.45rem 0.9rem; border-radius:6px;
                 cursor:pointer; margin-top:0.5rem; }
        a { color:#3b82f6; font-size:0.85rem; }
        .empty { color:var(--muted); padding:1rem 0; }
        </style></head><body>
        <h1>External partner integrator</h1>
        <p class="sub">Step 5 — webhook POSTs arrive here (HMAC verified)</p>
        <span class="badge">partner-app · :5102</span>

        <div class="card">
          <div>Events received: <span class="count" id="count">0</span></div>
          <button onclick="refresh()">Refresh</button>
          <label style="margin-top:0.75rem;display:block"><input type="checkbox" id="auto" checked/> Auto-refresh every 2s</label>
          <div id="tableWrap" style="margin-top:1rem"></div>
        </div>

        <p style="margin-top:1rem">
          <a href="http://localhost:5101/">Emit from source</a> ·
          <a href="http://localhost:8080/">Service Bus panel</a>
        </p>

        <script>
        let timer;
        async function refresh() {
          const r = await fetch('/received');
          const data = await r.json();
          document.getElementById('count').textContent = data.length;
          const wrap = document.getElementById('tableWrap');
          if (!data.length) {
            wrap.innerHTML = '<p class="empty">No events yet. Emit from Source app (5101).</p>';
            return;
          }
          let html = '<table><tr><th>Event ID</th><th>Received</th><th>Payload</th></tr>';
          for (const row of data) {
            html += '<tr><td>' + (row.eventId || '') + '</td><td>' + row.at + '</td><td><code>' +
              (row.body || '').replace(/</g,'&lt;') + '</code></td></tr>';
          }
          html += '</table>';
          wrap.innerHTML = html;
        }
        function schedule() {
          clearInterval(timer);
          if (document.getElementById('auto').checked)
            timer = setInterval(refresh, 2000);
        }
        document.getElementById('auto').onchange = schedule;
        refresh();
        schedule();
        </script>
        </body></html>
        """;
}
