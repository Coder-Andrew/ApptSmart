import Link from "next/link";
import styles from "@/app/page.module.css"
import WelcomeBanner from "@/components/home/WelcomeBanner";
import InfoPanel from "@/components/home/InfoPanel";
import RequireAuthFromQuery from "@/components/UI/Auth/RequireAuthFromQuery";


export default function Home() {

  return (
    <>
      <RequireAuthFromQuery />
      <WelcomeBanner />
      <InfoPanel />
    </>
  );
}
