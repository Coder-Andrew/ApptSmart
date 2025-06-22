import { NextRequest, NextResponse } from "next/server";

interface RouteContext {
  params: Promise<{
    path: string[];
  }>;
}

async function handler(req: NextRequest, context: RouteContext) {
  const backendURL = process.env.BACKEND_URL;
  if (!backendURL) {
    throw new Error("BACKEND_URL is not set. Please configure it in your environment variables.");
  }

  const params = await context.params;
  const backendPath = params.path.join("/");
  const url = `${backendURL}/api/${backendPath}${req.nextUrl.search}`;

  try {
    const res = await fetch(url, {
      method: req.method,
      credentials: "include",
      headers: {
        ...Object.fromEntries(req.headers.entries()),
        host: new URL(backendURL).host,
      },
      body: ["GET", "DELETE"].includes(req.method) ? undefined : await req.text(),
    });

    const isJson = res.headers.get("Content-Type")?.includes("application/json");
    const data = isJson ? await res.json() : await res.text();

    return isJson
      ? NextResponse.json(data, { status: res.status, headers: res.headers })
      : new NextResponse(data, { status: res.status, headers: res.headers });

  } catch {
    return NextResponse.json({ error: "Failed to fetch from backend" }, { status: 500 });
  }
}

export async function GET(req: NextRequest, context: RouteContext) {
  return handler(req, context);
}

export async function POST(req: NextRequest, context: RouteContext) {
  return handler(req, context);
}

export async function PUT(req: NextRequest, context: RouteContext) {
  return handler(req, context);
}

export async function DELETE(req: NextRequest, context: RouteContext) {
  return handler(req, context);
}
