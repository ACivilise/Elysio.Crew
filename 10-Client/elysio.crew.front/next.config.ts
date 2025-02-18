import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  env: {
    NEXT_PUBLIC_URL_API: process.env.services__api__https__0,
    NEXT_PUBLIC_CLIENT_ID: process.env.CLIENT_ID,
    NEXT_PUBLIC_TENANT_ID: process.env.TENANT_ID,
    NEXT_PUBLIC_API_SCOPE: process.env.API_SCOPE,
  },
  output: "standalone",
  dev: process.env.NODE_ENV !== "production",
};

export default nextConfig;
