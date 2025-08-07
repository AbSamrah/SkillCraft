import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// Create a custom trend metric to track login duration.
const getRoadmapDuration = new Trend('get_Roadmap_duration');

// --- Test Options for a Load Test ---
// We define the options directly, without the scenarios object.
export const options = {
    executor: 'ramping-vus',
    stages: [
        { duration: '2m', target: 100 }, // below
        { duration: '5m', target: 100 },
        { duration: '2m', target: 200 }, // normal
        { duration: '5m', target: 200 },
        { duration: '2m', target: 300 }, // around breaking point
        { duration: '5m', target: 300 },
        { duration: '2m', target: 400 }, // beyond breaking point
        { duration: '5m', target: 400 },
        { duration: '10m', target: 0 }, // scale down
    ],
    gracefulRampDown: '30s',
    thresholds: {
        'http_req_failed': ['rate<0.01'],   // http errors should be less than 1%
        'http_req_duration': ['p(95)<500'], // 95% of requests should be below 500ms
        'get_Roadmap_duration': ['p(95)<800'],   // 95% of login operations should be below 800ms
    },
};

// --- The Main Test Function ---
export default function () {
    const url = 'http://localhost:5093/api/Roadmaps/688bec85c33369e6d1b0dffc';


    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const res = http.get(url, params);

    getRoadmapDuration.add(res.timings.duration);

    check(res, {
        'get roadmap successful (status 200)': (r) => r.status === 200,
        'response body is roadmap': (r) => r.body.length > 100,
    });

    sleep(1);
}