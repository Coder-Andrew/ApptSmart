import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  // TODO: REMOVE IN PROD
  // TODO: REMOVE IN PROD
  // TODO: REMOVE IN PROD
  async headers() {
    return [
      {
        source: "/(.*)", // All routes
        headers: [
          {
            key: "Cache-Control",
            value: "no-store, no-cache, must-revalidate, proxy-revalidate",
          },
          {
            key: "Pragma",
            value: "no-cache",
          },
          {
            key: "Expires",
            value: "0",
          },
        ],
      },
    ];
  }
};

export default nextConfig;
