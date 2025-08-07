import { getAllRoadmapsTest } from '../common.js';

// --- Test Options for a Load Test ---
// We define the options directly, without the scenarios object.
export const options = {
    executor: 'ramping-vus',
    stages:
    [
        { duration: '1m', target: 200 }, // Start with 200 users
        { duration: '10s', target: 1000 }, // Spike to 1000 users
        { duration: '1m', target: 1000 }, // Hold
        { duration: '10s', target: 0 }, // Ramp down to 0
    ],
    gracefulRampDown: '30s',
    thresholds:
    {
    'http_req_failed': ['rate<0.01'],   // http errors should be less than 1%
        'http_req_duration': ['p(95)<500'], // 95% of requests should be below 500ms
        'get_all_roadmaps_duration': ['p(95)<800'],   // 95% of login operations should be below 800ms
    },
};

// --- The Main Test Function ---
export default getAllRoadmapsTest;