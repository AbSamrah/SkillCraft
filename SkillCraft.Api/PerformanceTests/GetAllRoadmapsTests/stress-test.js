import { getAllRoadmapsTest } from '../common.js';

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
export default getAllRoadmapsTest;