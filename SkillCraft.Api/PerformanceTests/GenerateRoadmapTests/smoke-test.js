import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// Create a custom trend metric to track login duration.
const generateRoadmapDuration = new Trend('generate_roadmap_duration');

// --- Test Options for a Load Test ---
// We define the options directly, without the scenarios object.
export const options = {
    executor: 'constant-vus',
    vus: 1,
    duration: '30s',
    thresholds: {
        'http_req_failed': ['rate<0.1'], // Allow a 10% failure rate for this complex task
        'http_req_duration': ['p(95)<120000'], // 95% of all requests must complete within 2 minutes
        'generate_roadmap_duration': ['p(90)<90000'],// 90% should be under 90 seconds
    },
};

// --- The Main Test Function ---
export default function () {
    const url = 'http://localhost:5093/api/Roadmaps/ai';

    // A list of diverse topics for the AI to generate
    const topics = ['React Native', 'Data Science', 'Backend Development with Go', 'DevOps Essentials', 'Machine Learning'];
    const randomTopic = topics[Math.floor(Math.random() * topics.length)];

    const payload = JSON.stringify({
        prompt: randomTopic,
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
            // IMPORTANT: You MUST replace this with a valid JWT from a logged-in user
            'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImZhMTI4ZTkxLWFiY2UtNGI3Zi04ZGQ1LTI1ODAyMWVhYTViMCIsImdpdmVuX25hbWUiOiJKYWZhciIsImZhbWlseV9uYW1lIjoiTWFobW9vZCIsImVtYWlsIjoiamFmYXJAZ21haWwuY29tIiwicm9sZSI6IlVzZXIiLCJleHAiOjE3NTQ0ODU4MjUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcxNTgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MTU4In0.Dk_5l7X0YwdvH2Xrp1wL5oE_Qmw1CakSXQ34kKlfYpg',
        },
        // Set a timeout to prevent requests from running forever
        timeout: '130s', // 2 minutes and 10 seconds
    };

    const res = http.post(url, payload, params);

    // Add the request duration to our custom metric
    generateRoadmapDuration.add(res.timings.duration);

    check(res, {
        'roadmap generated (status 201 Created)': (r) => r.status === 201,
        'response time is under 2 minutes': (r) => r.timings.duration < 120000,
    });

    // Wait a bit before the next VU starts its iteration
    sleep(5);
}