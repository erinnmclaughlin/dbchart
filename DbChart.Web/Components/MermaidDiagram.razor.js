import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@11/dist/mermaid.esm.min.mjs';
export async function init() {
    mermaid.initialize({ startOnLoad: false });
    await mermaid.run({
        querySelector: '.dbchart',
    });
}
